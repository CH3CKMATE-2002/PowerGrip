import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.tsx';
import './index.scss';
import { getLogger } from '~/utilities/Logging.ts';
import 'bootstrap/dist/css/bootstrap.min.css'; // Import bootstrap's css once.

const logger = getLogger();

logger.debug('Rendering UI.');

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
);
