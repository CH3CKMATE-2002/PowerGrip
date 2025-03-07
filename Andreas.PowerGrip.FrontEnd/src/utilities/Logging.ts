import settings from "~/appSettings";
import { formatTemplate } from "~/utilities/Templates";
import { ANSI_RESET, ansiRgb } from "~/utilities/AnsiConstants";
import LogLevel from "~/types/LogLevel";
import { LoggerSettings } from "~/types/Settings";
import { DateTime } from "luxon";

export const isBrowserEnv = () => {
    return typeof window !== 'undefined';
}

// @ts-ignore unused
const levelAsAdjustedString = (level: LogLevel) => {
    switch (level) {
        case LogLevel.SILENT:
            return ' Silent'; // Yea, never gonna see that.
        case LogLevel.ERROR:
            return ' Error ';
        case LogLevel.WARNING:
            return 'Warning';
        case LogLevel.DEBUG:
            return ' Debug ';
        case LogLevel.HINT:
            return ' Hint  ';
        case LogLevel.VERBOSE:
            return 'Verbose';
        default:
            throw new Error(`Level is out of range: ${level}`);
    }
};

const levelAsCuteString = (level: LogLevel) => {
    switch (level) {
        case LogLevel.SILENT:
            return '😶'; // Lol, never gonna see that.
        case LogLevel.ERROR:
            return '⛔';
        case LogLevel.WARNING:
            return '❗';
        case LogLevel.DEBUG:
            return '🎮';
        case LogLevel.HINT:
            return '💡';
        case LogLevel.VERBOSE:
            return '💬';
        default:
            throw new Error(`Level is out of range: ${level}`);
    }
};

export class Logger {
    private _messageFormat: string;
    public level: LogLevel;

    constructor(settings?: Partial<LoggerSettings>) {
        this.level = settings?.level ?? LogLevel.DEBUG;
        this._messageFormat = settings?.messageFormat ?? '<message>';
    }

    private formatMessage(level: LogLevel, message: string) {
        const result = formatTemplate(this._messageFormat, {
            timestamp: DateTime.now().toFormat('HH:mm:ss a'),
            level: levelAsCuteString(level),
            message,
        });

        return result;
    }

    public isSilent() {
        return this.level === LogLevel.SILENT;
    }

    public error(message: string | any) {
        const result = this.formatMessage(LogLevel.ERROR, message);
        if (this.level >= LogLevel.ERROR) {
            if (isBrowserEnv()) {
                console.error(result);
            } else {
                console.log(`%s${result}%s`, ansiRgb(255, 0, 0), ANSI_RESET);
            }
        }
    }

    public warning(message: string | any) {
        const result = this.formatMessage(LogLevel.WARNING, message);
        if (this.level >= LogLevel.WARNING) {
            if (isBrowserEnv()) {
                console.warn(result);
            } else {
                console.log(`%s${result}%s`, ansiRgb(245, 194, 17), ANSI_RESET);
            }
        }
    }

    public debug(message: string | any) {
        const result = this.formatMessage(LogLevel.DEBUG, message);
        if (this.level >= LogLevel.DEBUG) {
            if (isBrowserEnv()) {
                console.log(`%c${result}`, 'color: #22F511');
            } else {
                console.log(`%s${result}%s`, ansiRgb(51, 209, 122), ANSI_RESET);
            }
        }
    }

    public hint(message: string | any) {
        const result = this.formatMessage(LogLevel.HINT, message);
        if (this.level >= LogLevel.HINT) {
            if (isBrowserEnv()) {
                console.log(`%c${result}`, 'color: #1A72F1');
            } else {
                console.log(`%s${result}%s`, ansiRgb(26, 114, 241), ANSI_RESET);
            }
        }
    }

    public verbose(message: string | any) {
        const result = this.formatMessage(LogLevel.VERBOSE, message);
        if (this.level >= LogLevel.VERBOSE) {
            if (isBrowserEnv()) {
                console.log(`%c${result}`, 'color: grey');
            } else {
                console.log(`%s${result}%s`, ansiRgb(140, 140, 140), ANSI_RESET);
            }
        }
    }

    public data(level: LogLevel, message?: object | any) {
        if (this.level >= level) {
            console.log(message);
        }
    }
}

const defaultLogger = new Logger(settings.loggerSettings);

export const getLogger = () => {
    return defaultLogger;
};