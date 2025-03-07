import UbuntuLogoSvg from '~/assets/Icons/distros/ubuntu-logo-no-wordmark-solid-o-2022.svg';
import IconProps from '~/types/IconProps';

const Ubuntu = ({ width, height }: IconProps) => {
    return <img src={UbuntuLogoSvg} width={width} height={height} alt='ubuntu-logo' />
};

export default Ubuntu;