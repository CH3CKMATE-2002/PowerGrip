import { Container, Nav, Navbar, NavDropdown, Offcanvas } from "react-bootstrap";
import { PowerGripPLetter } from "~/components/Icons";
import { useLocation, useNavigate } from "react-router";
import { BsThemeSwitcher } from "~/components/BsThemeSwitcher";
import './AppNavBar.scss';

const AppNavBar = () => {
    const { pathname } = useLocation();
    const navigator = useNavigate();

    const toAbout = () => navigator('/about');
    const toLogin = () => navigator('/');

    return (
        <Navbar expand='lg' fixed="top" className="bg-body-tertiary">
            <Container fluid>
                <Navbar.Brand>
                    <Nav.Link onClick={toAbout}>
                        <PowerGripPLetter width={50} height={10} /> PowerGrip
                    </Nav.Link>
                </Navbar.Brand>

                <Navbar.Toggle aria-controls="basic-navbar-nav" />

                <Navbar.Offcanvas className='app-drawer'>
                    <Offcanvas.Header closeButton>
                        <Offcanvas.Title>
                            <Nav.Link onClick={toAbout}>
                                <PowerGripPLetter width={50} height={10} /> PowerGrip
                            </Nav.Link>
                        </Offcanvas.Title>
                    </Offcanvas.Header>

                    <Offcanvas.Body>
                        <Nav className="justify-content-end flex-grow-1 pe-3">

                            <BsThemeSwitcher />

                            <NavDropdown title="Quick Switch">
                                <NavDropdown.Item>Overview</NavDropdown.Item>
                                <NavDropdown.Item>Health</NavDropdown.Item>
                                <NavDropdown.Item>Services</NavDropdown.Item>
                                <NavDropdown.Item>Logs</NavDropdown.Item>
                                <NavDropdown.Item>Storage</NavDropdown.Item>
                                <NavDropdown.Item>Network</NavDropdown.Item>
                                <NavDropdown.Divider />
                                <NavDropdown.Item>Terminal</NavDropdown.Item>
                                <NavDropdown.Item>File Manager</NavDropdown.Item>
                                <NavDropdown.Item>Updates</NavDropdown.Item>
                            </NavDropdown>

                            <Nav.Link onClick={toLogin} disabled={pathname === '/'}>
                                Login
                            </Nav.Link>

                        </Nav>
                        {/* <Form className="d-flex">
                            <Form.Control
                                type="search"
                                placeholder="Search..."
                                className="me-2"
                                aria-label="Search"
                            />
                            <Button variant="outline-success">Search</Button>
                        </Form> */}
                    </Offcanvas.Body>

                </Navbar.Offcanvas>
            </Container>
        </Navbar>
    );
};

export default AppNavBar;