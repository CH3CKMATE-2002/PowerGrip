import { useState } from "react";
import { Button, Stack } from "react-bootstrap";
import { BsMoon, BsSun } from "react-icons/bs";
import { BsThemeName } from "~/types/Theming";
import { getLogger } from "~/utilities/Logging";
import { getUsedThemeName, setUsedThemeName } from "~/utilities/Storage";
import './ThemeSwitcher.scss';

const logger = getLogger();

const BsThemeSwitcher = () => {
    logger.verbose('Rendering ThemeSwitcher.');
    const [isDark, setIsDark] = useState(getUsedThemeName() === 'dark');

    const switchTheme = () => {
        setIsDark(!isDark);

        const newTheme: BsThemeName = isDark ? 'light' : 'dark';

        logger.verbose(`Switching to ${newTheme} theme.`);
        setUsedThemeName(newTheme);
    };

    return (
        <Button onClick={switchTheme} className="theme-switcher text-center" variant="outline-secondary">
            <Stack direction="horizontal">
                {isDark ? <BsMoon /> : <BsSun />}
                &nbsp;
                {isDark ? "Dark" : "Light"}
            </Stack>
        </Button>
    );
};

export default BsThemeSwitcher;