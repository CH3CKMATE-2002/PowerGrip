import { Card, Col, Row, Accordion } from "react-bootstrap";
// import { Alert } from "react-bootstrap";
// import { IoCheckmarkCircleOutline, IoCloseCircleOutline, IoWarningOutline } from "react-icons/io5";
// import { BsUbuntu } from "react-icons/bs"; // Small, suitable for as in-text icon only.
import { AccountLoginForm } from "~/components/AppForms";
import { ComingSoon } from "~/components/Banners";
import { BannerDisplay } from "~/components/Banners";
import { useEffect } from "react";

import Wallpaper from "~/assets/Wallpapers/beach-waves-night-dimmed.jpg";
import './LoginPage.scss';


const body = document.querySelector('body')!;

const LoginPage = () => {
    
    useEffect(() => {
        body.style.backgroundImage = `url(${Wallpaper})`;

        return () => {
            body.style.backgroundImage = '';
        };
    }, []);

    return (
        <>
            <Row>
                <Col md={6} className="mb-3">
                    <Card>
                        <Card.Header className="h1">
                            Login
                        </Card.Header>
                        <Card.Body>
                            <Card.Title>Login to the Dashboard</Card.Title>
                            <Card.Subtitle className="text-muted">
                                Choose desired login method.
                            </Card.Subtitle>
                            <hr />
                            <Card.Text as='div'>
                                {/* As div, to remove that warning about <h2> cannot be a child of <p> */}
                                <Accordion defaultActiveKey={['0']}>
                                    <Accordion.Item eventKey="0">
                                        <Accordion.Header>Account Login</Accordion.Header>
                                        <Accordion.Body>
                                            {/* Here's where the user is going to login. */}
                                            <AccountLoginForm />
                                        </Accordion.Body>
                                    </Accordion.Item>
                                    <Accordion.Item eventKey="1">
                                        <Accordion.Header>SSH Login</Accordion.Header>
                                        <Accordion.Body>
                                            {/* TODO: SSH Support? */}
                                            <ComingSoon />
                                        </Accordion.Body>
                                    </Accordion.Item>
                                </Accordion>
                            </Card.Text>
                        </Card.Body>
                        <Card.Footer className="text-muted text-light">
                            Your server awaits you.
                        </Card.Footer>
                    </Card>
                </Col>
                <Col>
                    <BannerDisplay />
                    {/* <Alert variant="danger" className="mt-3">
                        <Alert.Heading><IoCloseCircleOutline /> Yikes!</Alert.Heading>
                        That would sound really bad if it wasn't a placeholder...
                    </Alert>
                    <Alert variant="warning">
                        <Alert.Heading><IoWarningOutline/> Whoopsie!</Alert.Heading>
                        That would sound really annoying if it wasn't a placeholder...
                    </Alert>
                    <Alert variant="success">
                        <Alert.Heading><IoCheckmarkCircleOutline /> Yippee!</Alert.Heading>
                        That would sound really joyful if it wasn't a placeholder...
                    </Alert> */}
                </Col>
            </Row>
        </>
    );
};

export default LoginPage;