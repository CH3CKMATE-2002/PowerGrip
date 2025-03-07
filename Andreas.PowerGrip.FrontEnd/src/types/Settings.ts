import LogLevel from "./LogLevel";

export interface AppSettings {
    loggerSettings: LoggerSettings;
}

export interface LoggerSettings {
    messageFormat: string;
    level: LogLevel;
}