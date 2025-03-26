import { PowerView } from '~/components/PowerView';
import './DashboardPage.scss';
import { Col, Row } from 'react-bootstrap';

const DashboardPage = () => {
    return (
        <>
            <Row>
                <Col>
                    Hello
                </Col>
                <Col>
                    <PowerView />
                </Col>
            </Row>
        </>
    );
};

export default DashboardPage;