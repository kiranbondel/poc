declare global {
  interface Window {
    RUNTIME_CONFIG: {
      REACT_APP_API_BASE_URL: string;
    };
  }
}

const config = {
  API_BASE_URL: window.RUNTIME_CONFIG?.REACT_APP_API_BASE_URL || 'https://localhost:5001',
};

export default config;