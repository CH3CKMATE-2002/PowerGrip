import { FamousOsName } from "~/types/FamousOsName";
import UbuntuBanner from "./UbuntuBanner";

const BannerDisplay = () => {
    const OS: FamousOsName = 'ubuntu'; // TODO: API call to server to determine server's OS

    return (
        <div style={{ borderRadius: '25px' }} className="bg-body border border-2">
            {OS === 'ubuntu' && <UbuntuBanner />}
        </div>
    );
};

export default BannerDisplay;