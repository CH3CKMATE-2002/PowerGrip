import { BsThemeName } from "~/types/Theming";
import { getLogger } from "./Logging";

const logger = getLogger();
const BOOTSTRAP_THEME_ATTR = 'data-bs-theme';

const page: HTMLHtmlElement = document.querySelector('html')!;

export const getUsedThemeName = (): BsThemeName => {
    const val = <BsThemeName | null> page.getAttribute(BOOTSTRAP_THEME_ATTR);

    if (!val || val !== 'dark' && val !== 'light') {
        logger.verbose('Theme name is not yet set or invalid, fixing...');
        page.setAttribute(BOOTSTRAP_THEME_ATTR, 'dark');
        return 'dark';
    }

    return val;
};

export const setUsedThemeName = (themeName: BsThemeName) => {
    page.setAttribute(BOOTSTRAP_THEME_ATTR, themeName);
};