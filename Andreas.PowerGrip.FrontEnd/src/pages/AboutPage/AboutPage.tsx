import { Card, Col, Container, Row } from 'react-bootstrap';
import { randomElement } from '~/utilities/Miscellaneous';
import './AboutPage.scss';

const ADJECTIVES = [
    'good',
    'all-mighty',
    'strong',
    'like having a good time',
    'monstrous',
    'satisfied',
    'happy',
    'productive',
    'genius',
    'the power',
    'the strength of your grip'
];

const AboutPage = () => {
    const CHOSEN_ADJ = randomElement(ADJECTIVES);

    return (
        <>
            {/* TODO: Banner? */}
            <Container className='text-center'>
                <h1>
                    PowerGrip!
                </h1>
                <p>
                    A linux server dashboard with small, yet powerful AI tools
                    that will make you feel <span className='text-decoration-underline'>{CHOSEN_ADJ}</span>!
                </p>
            </Container>
            <hr />
            <fieldset className='text-center'> {/* This sets common attributes for its children */}
                <Row>
                    <Col>
                        <Card>
                            <Card.Header as='h1'>
                                💪All-Powerful
                            </Card.Header>
                            <Card.Body>
                                <Card.Text>
                                    As you getting familiar with PowerGrip, you'll notice
                                    the power under your control. PowerGrip comes with powerful
                                    tools, ranging from simple ones to the AI powered. Seems like
                                    managing your server will be a joyful ride!
                                </Card.Text>
                            </Card.Body>
                        </Card>
                    </Col>
                    <Col>
                        <Card>
                            <Card.Header as='h1'>
                                🔥Blazing Fast
                            </Card.Header>
                            <Card.Body>
                                <Card.Text>
                                    With the help of .NET 8 and React, you'll have
                                    control over your server with a responsive dashboard
                                    and quite fast backend server suitable for all needs.
                                </Card.Text>
                            </Card.Body>
                        </Card>
                    </Col>
                    <Col>
                        <Card>
                            <Card.Header as='h1'>
                                😍Forged with Love
                            </Card.Header>
                            <Card.Body>
                                <Card.Text>
                                    PowerGrip started as a personal project of <b>Andreas P. Hanna</b>,
                                    who later, wanted to share it to all the world as a free, open-source,
                                    application to all the world for personal use.
                                </Card.Text>
                            </Card.Body>
                        </Card>
                    </Col>
                </Row>

                <Row className='mt-3 mb-3'>
                    <Col>
                        <Card>
                            <Card.Header as='h1'>
                                🛡Secure
                            </Card.Header>
                            <Card.Body>
                                <Card.Text>
                                    PowerGrip was built with security in mind.
                                    You login using the application's password to access the
                                    dashboard, but further tasks would require your system account's
                                    password to complete. You might also want to login with
                                    ssh keys instead of the old fashioned password! Did you know that
                                    you can also customize the login methods in the login screen?
                                </Card.Text>
                            </Card.Body>
                        </Card>
                    </Col>
                    <Col>
                        <Card>
                            <Card.Header as='h1'>
                                🧠Smart
                            </Card.Header>
                            <Card.Body>
                                <Card.Text>
                                    Despite being powered with simple AI tools, PowerGrip is quite
                                    smart, it learns from you! After learning quite well, PowerGrip will
                                    be able to tell the anomalies quite quickly and accurately, propose system
                                    configuration and fixes, and even give you hints for your use-case!
                                </Card.Text>
                            </Card.Body>
                        </Card>
                    </Col>
                    <Col>
                        <Card>
                            <Card.Header as='h1'>
                                🎨Colorful
                            </Card.Header>
                            <Card.Body>
                                <Card.Text>
                                    With the help of Bootstrap and React-Bootstrap, PowerGrip has a
                                    mature, modern-looking UI.
                                    PowerGrip sports both light and dark themes, suitable for
                                    every light hero or dark dweller.
                                    Who said that a linux dashboard has to be an eye-sore?
                                </Card.Text>
                            </Card.Body>
                        </Card>
                    </Col>
                </Row>
            </fieldset>
        </>
    );
};

export default AboutPage;