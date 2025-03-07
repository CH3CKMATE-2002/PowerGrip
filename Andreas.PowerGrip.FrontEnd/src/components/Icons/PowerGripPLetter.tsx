import { Image } from 'react-bootstrap';
import PowerGripSvg from '~/assets/Icons/power-grip/power-grip.svg';
import IconProps from '~/types/IconProps';

const PowerGripPLetter = ({ width, height }: IconProps) => {
    return <Image src={PowerGripSvg} fluid width={width} height={height} alt='power-grip' />;
};

export default PowerGripPLetter;