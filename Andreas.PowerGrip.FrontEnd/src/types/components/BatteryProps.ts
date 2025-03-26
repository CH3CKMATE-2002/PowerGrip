export default interface BatteryProps {
    percentage: number;
    charging?: boolean;
    direction?: "horizontal" | "vertical"
};