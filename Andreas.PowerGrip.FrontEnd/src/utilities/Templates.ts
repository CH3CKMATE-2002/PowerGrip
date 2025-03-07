export const formatTemplate = (template: string, record: Record<string, any>) => {
    return template.replace(/<\w+>/g, (placeholder) => {
        const key = placeholder.slice(1, -1); // Does not include the last char!
        return record[key] || placeholder;
    });
};