import { ReactNode } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';

import { MainLayout } from '~/components/MainLayout';
import { AboutPage, LoginPage, DashboardPage } from '~/pages';
import './App.scss';

// Define pages and routes here
const pages: Array<{ route: string, page: ReactNode }> = [
  { route: "/", page: <LoginPage /> },
  { route: "/about", page: <AboutPage /> },
  { route: "/dashboard", page: <DashboardPage/> }
];

const App = () => {
  return (
    <>
      <BrowserRouter>
        {/* The MainLayout renders the AppNavBar which uses BrowserRouter's useNavigate. */}
        <MainLayout>
          <Routes>
            {pages.map(({ route, page }) => {
              return <Route path={route} element={page} key={route} />;
            })}
          </Routes>
        </MainLayout>
      </BrowserRouter>
    </>
  );
};

export default App;
