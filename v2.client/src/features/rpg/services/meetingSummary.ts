const stopWords = new Set((
    'a aby ach albo ale ani aż bardziej bez bo być była były by będzie będziemy będziecie będą będę cały cała całe co czy dla do gdzie gdy i ich ile im inna inne inny już jak jako jeden jedna jego jej jeśli jest jeszcze ją je jemu jeśli każdy kiedy kilka kto która które który ku lub ma mają mam między mnie może na nad nam nami nas nasza nasze nasz nie nich nic no o od on ona one oni ono oraz po pod podczas pomimo ponad poprzez przy przed przez raczej robili robić są sam sama samo się siebie sobie sobie stać taki takie tak to tu tym tych u w we więc więc właśnie warto w was wasz wasza wasze wśród z za ze że'.toLowerCase()
).split(/\s+/));

/** Builds a small language-independent extractive summary without sending text off-device. */
export function generateMeetingSummary(transcript: string, maxSentences = 4): string {
    const sentences = transcript.match(/[^.!?\n]+[.!?]?/g)?.map(value => value.trim()).filter(Boolean) ?? [];
    if (!sentences.length) return '';
    if (sentences.length <= maxSentences) return sentences.join(' ');

    const words = (text: string) => text.toLocaleLowerCase().match(/[\p{L}\p{N}]{3,}/gu) ?? [];
    const frequency = new Map<string, number>();
    sentences.forEach(sentence => words(sentence).forEach(word => {
        if (!stopWords.has(word)) frequency.set(word, (frequency.get(word) ?? 0) + 1);
    }));
    const ranked = sentences.map((sentence, index) => {
        const terms = words(sentence).filter(word => !stopWords.has(word));
        const score = terms.reduce((sum, word) => sum + (frequency.get(word) ?? 0), 0) / Math.sqrt(Math.max(terms.length, 1));
        return { index, score };
    });
    return ranked.sort((a, b) => b.score - a.score).slice(0, maxSentences)
        .sort((a, b) => a.index - b.index).map(item => sentences[item.index]).join(' ');
}
