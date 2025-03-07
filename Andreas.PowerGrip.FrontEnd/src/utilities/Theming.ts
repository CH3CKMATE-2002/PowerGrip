import { BsThemeName } from "~/types/Theming";

export const antiThemeName = (themeName: BsThemeName): BsThemeName => {
    switch (themeName) {
        case 'dark':
            return 'light';
        case 'light':
            return 'dark';
    }
};