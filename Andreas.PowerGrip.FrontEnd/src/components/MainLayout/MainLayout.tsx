import { Container } from "react-bootstrap";
import { AppNavbar } from "../AppNavBar";


interface MainLayoutProps {
    children?: React.ReactNode
}

const MainLayout = ({ children }: MainLayoutProps) => {
    return (
        <>
            <AppNavbar />
            {/* The paddingTop property is set because of the navbar */}
            <Container fluid style={{ paddingTop: 100 }}>
                {children}
            </Container>
        </>
    );
};

export default MainLayout;