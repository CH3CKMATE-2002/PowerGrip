import LogLevel from "./types/LogLevel";
import { AppSettings } from "./types/Settings"

const appSettings: AppSettings = {
    loggerSettings: {
        messageFormat: '[<level> @ <timestamp>]: <message>',
        level: LogLevel.VERBOSE,
    },
};

export default appSettings;