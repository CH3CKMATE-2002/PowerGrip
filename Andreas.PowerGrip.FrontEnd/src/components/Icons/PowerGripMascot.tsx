import { Image } from 'react-bootstrap';
import PowerGripMascotSvg from '~/assets/Icons/power-grip/power-grip-mascot.svg';
import IconProps from '~/types/IconProps';

const PowerGripMascot = ({ width, height }: IconProps) => {
    return <Image src={PowerGripMascotSvg} fluid width={width} height={height} alt='mascot' />;
};

export default PowerGripMascot;