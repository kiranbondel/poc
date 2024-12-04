import { BrowserRouter as Router, Routes, Route, useNavigate, useLocation } from 'react-router-dom';
import { Tabs, Tab, Box } from '@mui/material';
import Cafe from './pages/Cafe';
import Employee from './pages/Employee';
import AddEditCafe from './pages/AddEditCafe.tsx';
import AddEditEmployee from './pages/AddEditEmployee.tsx';

const AppTabs = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const getActiveTab = () => {
    if (location.pathname === '/cafes' || location.pathname === '/') return 0;
    if (location.pathname === '/employees') return 1;
    if (location.pathname === '/add-edit-cafe') return 2;
    if (location.pathname === '/add-edit-employee') return 3;
    return 0;
  };

  const handleTabChange = (newValue: number) => {
    if (newValue === 0) navigate('/cafes');
    if (newValue === 1) navigate('/employees');
    if (newValue === 2) navigate('/add-edit-cafe');
    if (newValue === 3) navigate('/add-edit-employee');
  };

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        width: '100%',
        minHeight: '100vh',
        padding: '20px',
      }}
    >
      <Tabs value={getActiveTab()} onChange={(_, newValue) => handleTabChange(newValue)} centered>
        <Tab label="Cafes" />
        <Tab label="Employees" />
        <Tab label="Add/Edit Cafe" />
        <Tab label="Add/Edit Employee" />
      </Tabs>

      <Box mt={2} sx={{ width: '100%' }}>
        <Routes>
          <Route path="/" element={<Cafe />} />
          <Route path="/cafes" element={<Cafe />} />
          <Route path="/employees" element={<Employee />} />
          <Route path="/add-edit-cafe" element={<AddEditCafe />} />
          <Route path="/add-edit-employee" element={<AddEditEmployee />} />
        </Routes>
      </Box>
    </Box>
  );
};

const App = () => (
  <Router>
    <AppTabs />
  </Router>
);

export default App;
