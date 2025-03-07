import { Stack } from "react-bootstrap";
import { Ubuntu } from "~/components/Icons";
import { BannerProps } from "~/types/components";

const UbuntuBanner = ({ height = 60 }: BannerProps) => {
    return (
        <Stack direction="horizontal">
            <Ubuntu height={height} />
            &nbsp;
            <span style={{ fontSize: height }}>Ubuntu</span>
        </Stack>
    );
};

export default UbuntuBanner;