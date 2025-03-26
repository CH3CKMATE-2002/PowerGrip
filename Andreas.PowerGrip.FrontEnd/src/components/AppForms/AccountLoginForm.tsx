import { FormEvent, useState } from "react";
import { Button, Container, Form, Row, Col } from "react-bootstrap";
import { useNavigate } from "react-router";
import { PasswordInput } from "~/components/PasswordInput";
import LogLevel from "~/types/LogLevel";
import { getLogger } from "~/utilities/Logging";


const logger = getLogger();


const AccountLoginForm = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    
    const navigator = useNavigate();

    const handleSubmit = (e: FormEvent) => {
        e.preventDefault();
        logger.verbose('User is submitting form.');
        logger.data(LogLevel.VERBOSE, { username, password });

        logger.debug("Navigating to Main UI");
        navigator("/dashboard");
    };

    return (
        <Form onSubmit={handleSubmit}>
            <Container>
                <Form.Group className="mb-3" controlId="formBasicEmail" as={Row}>
                    <Form.Label column sm={3}>Username</Form.Label>
                    <Col sm={9}>
                        <Form.Control
                            type="username"
                            value={username}
                            onChange={e => setUsername(e.target.value)}
                            placeholder="Enter Your Username" />
                    </Col>
                </Form.Group>

                <Form.Group className="mb-3" controlId="formBasicPassword" as={Row}>
                    <Form.Label column sm={3}>Password</Form.Label>
                    <Col sm={9}>
                        <PasswordInput
                            value={password}
                            onChange={e => setPassword(e)} />
                    </Col>
                </Form.Group>
            </Container>

            <Button variant="outline-success" type="submit">
                Login
            </Button>
        </Form>
    );
};

export default AccountLoginForm;