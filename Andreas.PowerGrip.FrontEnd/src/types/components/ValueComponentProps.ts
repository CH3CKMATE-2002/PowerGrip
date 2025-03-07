export default interface ValueComponentProps<T> {
    value?: T;
    onChange?: (newValue: T) => void;
}