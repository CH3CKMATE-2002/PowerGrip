import { ProgressBar } from "react-bootstrap";
import { BsLightning } from "react-icons/bs";
import { BatteryProps } from "~/types/components";
import { clamp } from "~/utilities/Miscellaneous";
import './Battery.scss';

const getVariant = (percentage: number): string => {
    return percentage > 50 ? 'success' :
                percentage > 20 ? 'warning' : 'danger';
};

const Battery = ({ percentage, charging = false, direction = "horizontal" }: BatteryProps) => {
    const clampedPercentage = clamp(percentage, 0, 100);
    const variant = getVariant(percentage);
    const isVertical = direction === "vertical";

    return (
        <>
            <div className={`battery-container ${isVertical ? "vertical" : ""}`}>
                <div className="battery-cap"></div>
                <div className="battery-bar">
                    <ProgressBar
                        className="battery-juice"
                        now={clampedPercentage}
                        variant={variant}
                        label={
                            <span className="battery-text">
                                {clampedPercentage}% {charging && <BsLightning />}
                            </span>
                        }
                    />
                </div>
            </div>
        </>
    );
};

export default Battery;