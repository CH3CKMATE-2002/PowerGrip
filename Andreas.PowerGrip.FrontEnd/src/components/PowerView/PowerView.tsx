import { Card, Col, Row } from 'react-bootstrap';
import { Battery } from '~/components/Battery';
import { BsHeart } from 'react-icons/bs';
import './PowerView.scss';

const batteries: Array<{ name: string, percentage: number, health: number, isCharging: boolean }> = [
    { name: "BAT1", percentage: 100, health: 50.4, isCharging: true },
    { name: "BAT1", percentage: 50, health: 100, isCharging: false },
];

const PowerView = () => {
    return (
        <>
            <Card>
                <Card.Header as='h1'>
                    Power Supplies
                </Card.Header>
                <Card.Body>
                    <Row>
                        {batteries.map(({ name, percentage, health, isCharging }) =>
                        <div key={name}>
                            <Col>
                                <Battery percentage={percentage} charging={isCharging} />
                            </Col>
                            <p><BsHeart /> : {health}%</p>
                        </div>)}
                    </Row>
                </Card.Body>
            </Card>
        </>
    );
};

export default PowerView;