import './App.scss';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { MainLayout } from '~/components/MainLayout';
import { AboutPage, LoginPage } from '~/pages';

const App = () => {
  return (
    <>
      <BrowserRouter>
        {/* The MainLayout renders the AppNavBar which uses BrowserRouter's useNavigate. */}
        <MainLayout>
          <Routes>
            <Route path="/" element={<LoginPage />} />
            <Route path="/about" element={<AboutPage />} />
          </Routes>
        </MainLayout>
      </BrowserRouter>
    </>
  );
};

export default App;
