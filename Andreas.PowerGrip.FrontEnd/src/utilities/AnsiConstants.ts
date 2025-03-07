export const ANSI_ESC = '\x1B[';
export const ANSI_RESET = `${ANSI_ESC}0m`;

export const ansiRgb = (r: number, g: number, b: number) => {
    return `${ANSI_ESC}38;2;${r};${g};${b}m`;
};