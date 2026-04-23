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
            header.setStyle('font-weight', '600');
            header.setStyle('margin', '8px 0 4px');
            header.setStyle('font-size', '12px');
            header.setStyle('opacity', '0.7');
            header.setText(label);

            panel.append(header);

            items.forEach(item => {
                const el = new CKEDITOR.dom.element('div');

                el.setStyles({
                    padding: '8px',
                    cursor: 'pointer',
                    borderRadius: '6px',
                    marginBottom: '2px'
                });

                el.setHtml(`
            <div style="font-size:14px; font-weight:600;">
                ${item.title}
            </div>
            <div style="font-size:12px; opacity:0.8;">
                ${item.invoker}
            </div>
            ${item.description
                        ? `<div style="font-size:11px; color:#888; margin-top:2px;">
                        ${item.description}
                      </div>`
                        : ''
                    }
        `);

                // hover effect
                el.on('mouseover', function () {
                    el.setStyle('background', '#f5f5f5');
                });

                el.on('mouseout', function () {
                    el.removeStyle('background');
                });

                el.on('click', function () {
                    insertSuggestion(item);
                });

                panel.append(el);
            });
        }

        function renderSuggestions(data) {
            clearPanel();

            const labels = getLabels();

            flatItems = [];
            activeIndex = -1;

            const pushGroup = (label, items) => {
                if (!items || !items.length) return;

                const header = new CKEDITOR.dom.element('div');
                header.setStyle('font-weight', '600');
                header.setStyle('margin', '8px 0 4px');
                header.setStyle('font-size', '12px');
                header.setStyle('opacity', '0.7');
                header.setText(label);

                panel.append(header);

                items.forEach(item => {
                    const index = flatItems.length;
                    flatItems.push(item);

                    const el = createItemElement(item, index);
                    panel.append(el);
                });
            };

            pushGroup(labels.functions, data.functions);
            pushGroup(labels.variables, data.variables);

            if (!flatItems.length) {
                const empty = new CKEDITOR.dom.element('div');
                empty.setStyle('padding', '6px');
                empty.setText(labels.empty);
                panel.append(empty);
            }

            panel.show();
            positionPanel();
        }

        let itemElements = [];

        function createItemElement(item, index) {
            const el = new CKEDITOR.dom.element('div');

            el.setAttribute('data-index', index);

            el.setStyles({
                padding: '8px',
                cursor: 'pointer',
                borderRadius: '6px'
            });

            el.setHtml(`
                <div style="font-size:14px; font-weight:600;">
                    ${item.title}
                </div>
                <div style="font-size:12px; opacity:0.8; font-family:monospace;">
                    ${item.invoker}
                </div>
                ${item.description
                    ? `<div style="font-size:11px; color:#888; margin-top:2px;">
                            ${item.description}
                        </div>`
                    : ''
                }
            `);

            el.on('mouseover', () => setActive(index));
            el.on('click', () => insertSuggestion(item));

            itemElements[index] = el;

            return el;
        }


        function setActive(index) {
            if (index < 0 || index >= itemElements.length) return;

            // remove previous
            if (activeIndex >= 0 && itemElements[activeIndex]) {
                itemElements[activeIndex].removeStyle('background');
            }

            activeIndex = index;

            const el = itemElements[activeIndex];

            el.setStyle('background', '#eef3ff');

            // auto scroll into view
            const elNode = el.$;
            const panelNode = panel.$;

            if (elNode.offsetTop < panelNode.scrollTop) {
                panelNode.scrollTop = elNode.offsetTop;
            } else if (elNode.offsetTop + elNode.offsetHeight > panelNode.scrollTop + panelNode.clientHeight) {
                panelNode.scrollTop = elNode.offsetTop - panelNode.clientHeight + elNode.offsetHeight;
            }
        }

        editor.on('contentDom', function () {
            const doc = editor.document;

            if (!doc) return;

            // ✅ keydown (navigation)
            doc.on('keydown', function (evt) {
                const key = evt.data.getKey();

                if (!isActive) return;

                if (key === 40) { // ↓
                    evt.data.preventDefault();
                    setActive((activeIndex + 1) % flatItems.length);
                    return;
                }

                if (key === 38) { // ↑
                    evt.data.preventDefault();
                    setActive((activeIndex - 1 + flatItems.length) % flatItems.length);
                    return;
                }

                if (key === 13) { // ENTER
                    if (activeIndex >= 0) {
                        evt.data.preventDefault();
                        insertSuggestion(flatItems[activeIndex]);
                    }
                    return;
                }

                if (key === 27) { // ESC
                    cleanup();
                }
            });

            // ✅ keyup (typing)
            doc.on('keyup', function (evt) {
                const key = evt.data.getKey();

                if ([38, 40, 13].includes(key)) return;

                updateQuery();
            });

            // ✅ click
            doc.on('click', function () {
                cleanup();
            });
        });

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