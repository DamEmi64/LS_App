CKEDITOR.plugins.add('fluidSuggestion', {
    init: function (editor) {

        let panel = null;
        let markerRange = null;
        let isActive = false;
        let currentQuery = '';
        let cachedData = null;

        // ------------------------
        // Config helpers
        // ------------------------

        function getConfig() {
            return editor.config.fluidSuggestion || {};
        }

        function getLabels() {
            const labels = getConfig().labels || {};
            return {
                functions: labels.functions || 'Functions',
                variables: labels.variables || 'Variables',
                empty: labels.empty || 'No results'
            };
        }

        // ------------------------
        // Data loading (ONCE)
        // ------------------------

        function loadAllSuggestions() {
            const fn = getConfig().loadSuggestions;

            if (!cachedData && typeof fn === 'function') {
                return Promise.resolve(fn()).then(data => {
                    cachedData = data || { functions: [], variables: [] };
                    return cachedData;
                });
            }

            return Promise.resolve(cachedData || { functions: [], variables: [] });
        }

        // ------------------------
        // Filtering
        // ------------------------

        function filterItems(items, query) {
            if (!items) return [];

            if (!query) return items;

            const q = query.toLowerCase();

            return items.filter(item =>
                (item.title && item.title.toLowerCase().includes(q)) ||
                (item.invoker && item.invoker.toLowerCase().includes(q))
            );
        }

        function filterData(data, query) {
            return {
                functions: filterItems(data.functions, query),
                variables: filterItems(data.variables, query)
            };
        }

        // ------------------------
        // UI
        // ------------------------

        function createPanel() {
            panel = new CKEDITOR.dom.element('div');
            panel.setStyles({
                position: 'absolute',
                background: '#fff',
                border: '1px solid #ccc',
                padding: '6px',
                zIndex: 20000,
                maxHeight: '250px',
                overflowY: 'auto',
                minWidth: '220px',
                boxShadow: '0 2px 6px rgba(0,0,0,0.15)'
            });

            panel.hide();
            document.body.appendChild(panel.$);
        }

        function positionPanel() {
            const sel = editor.getSelection();
            if (!sel) return;

            const range = sel.getRanges()[0];
            if (!range) return;

            const rects = range.getClientRects();
            if (!rects || !rects.length) return;

            const caretRect = rects[0];

            // 👇 iframe element (CRITICAL)
            const iframe = editor.window.getFrame().$;
            const iframeRect = iframe.getBoundingClientRect();

            // 👇 final absolute position (page coordinates)
            const top = iframeRect.top + caretRect.bottom;
            const left = iframeRect.left + caretRect.left;

            panel.setStyles({
                position: 'absolute',
                top: top + 'px',
                left: left + 'px'
            });
        }

        function clearPanel() {
            if (panel) panel.setHtml('');
        }

        function renderGroup(label, items) {
            if (!items || !items.length) return;

            const header = new CKEDITOR.dom.element('div');
            header.setStyle('font-weight', 'bold');
            header.setStyle('margin', '6px 0 2px');
            header.setText(label);

            panel.append(header);

            items.forEach(item => {
                const el = new CKEDITOR.dom.element('div');
                el.setStyles({
                    padding: '4px',
                    cursor: 'pointer'
                });

                el.setHtml(
                    `<strong>${item.title}</strong><br/><small>${item.description || ''}</small>`
                );

                el.on('click', function () {
                    insertSuggestion(item);
                });

                panel.append(el);
            });
        }

        function renderSuggestions(data) {
            clearPanel();

            const labels = getLabels();

            const hasAny =
                (data.functions && data.functions.length) ||
                (data.variables && data.variables.length);

            if (!hasAny) {
                const empty = new CKEDITOR.dom.element('div');
                empty.setStyle('padding', '4px');
                empty.setText(labels.empty);
                panel.append(empty);
            } else {
                renderGroup(labels.functions, data.functions);
                renderGroup(labels.variables, data.variables);
            }

            panel.show();
            positionPanel();
        }

        // ------------------------
        // Insert logic
        // ------------------------

        function insertSuggestion(item) {
            if (!markerRange) return;

            editor.focus();

            const sel = editor.getSelection();
            sel.selectRanges([markerRange]);

            if (item.type == "function") {
                editor.insertText(`${item.invoker}()}}`);
            } else {
                editor.insertText(`${item.invoker}}}`);
            }

            cleanup();
        }

        function cleanup() {
            isActive = false;
            currentQuery = '';
            markerRange = null;

            if (panel) panel.hide();
        }

        // ------------------------
        // Query detection
        // ------------------------

        function updateQuery() {
            const sel = editor.getSelection();
            if (!sel) return;

            const range = sel.getRanges()[0];
            if (!range || !range.startContainer) return;

            const container = range.startContainer;

            if (container.type !== CKEDITOR.NODE_TEXT) {
                cleanup();
                return;
            }

            const text = container.getText();
            const before = text.substring(0, range.startOffset);

            const match = before.match(/{{([\w.]*)$/);

            if (match) {
                currentQuery = match[1];

                if (!isActive) {
                    isActive = true;
                    markerRange = range.clone();
                }

                loadAllSuggestions().then(data => {
                    const filtered = filterData(data, currentQuery);
                    renderSuggestions(filtered);
                });

            } else {
                cleanup();
            }
        }

        // ------------------------
        // Events
        // ------------------------

        editor.on('contentDom', function () {

            createPanel();

            editor.document.on('keyup', function (evt) {
                const key = evt.data.getKey();

                // ESC closes dropdown
                if (key === 27) {
                    cleanup();
                    return;
                }

                updateQuery();
            });

            editor.document.on('click', function () {
                cleanup();
            });

            editor.on('blur', function () {
                cleanup();
            });
        });
    }
});