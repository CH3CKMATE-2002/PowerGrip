import LogLevel from "~/types/LogLevel";
import { getLogger } from "./Logging";

const logger = getLogger();

/**
 * A function that generates a random number in a range between two given values.
 * @param min The minimum number that can be generated (Inclusive).
 * @param max The maximum number that can be generated (Inclusive).
 * @returns Random generated number in the range [min, max] (BOTH Inclusive).
 */
export const randomNumber = (min: number, max: number) => {
    const range = max - min;
    const result = min + Math.round(Math.random() * range);

    logger.verbose(`Rolling the dice [${min}, ${max}]: ${result}`);

    return result;
};

/**
 * A function that returns a random;y chosen element from an array.
 * @param arr The array to pick the element from.
 * @returns Random element from the given array.
 */
export const randomElement = <T>(arr: Array<T>): T => {
    const i = randomNumber(0, arr.length - 1);
    const result = arr[i];
    logger.verbose(`Element #${i} was chosen.`);
    logger.data(LogLevel.VERBOSE, result);
    return result;
}

export const clamp = (value: number, min: number, max: number): number => {
    return Math.max(min, Math.min(value, max));
};