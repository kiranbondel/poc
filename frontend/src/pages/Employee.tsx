import { useState, useEffect } from 'react';
import { AgGridReact, CustomCellRendererProps } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Snackbar, Alert, Card, CardContent } from '@mui/material';
import { useLocation, useNavigate } from 'react-router-dom';
import config from '../config';
import { ColDef } from 'ag-grid-community';

interface Employee {
  id: string;
  name: string;
  emailAddress: string;
  gender: string;
  phoneNumber: string;
  daysWorked: number;
  cafe: string;
}

const Employee = () => {

  const apiUrl = config.API_BASE_URL;
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [deleteId, setDeleteId] = useState('');
  const [showSnackbar, setShowSnackbar] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();
  const cafe = new URLSearchParams(location.search).get('cafe');

  useEffect(() => {
    fetchEmployees();
  }, []);

  const fetchEmployees = async () => {
    try {
      let url = `${apiUrl}/Employee`;
      if (cafe) url += `?cafe=${encodeURIComponent(cafe)}`;
      const response = await fetch(url);
      const data = await response.json();
      if (data.success && Array.isArray(data.result)) {
        setEmployees(data.result[0].payload);
      }
    } catch (error) {
      console.log('Error fetching employees:', error);
    }
  };

  const handleDeleteConfirmation = (id: string) => {
    setDeleteId(id);
    setOpenDialog(true);
  };

  const handleDeleteEmployee = async () => {
    try {
      const url = `${apiUrl}/Employee?Id=${encodeURIComponent(deleteId)}`;
      const response = await fetch(url, {
        method: 'DELETE',
      });

      if (response.ok) {
        setOpenDialog(false);
        setShowSnackbar(true);
        fetchEmployees();
      } else {
        console.error('Error deleting employee');
      }
    } catch (error) {
      console.error('Error deleting employee:', error);
    }
  };

  const handleEditEmployee = (id: string) => {
    const employeeToEdit = employees.find((emp) => emp.id === id);
    navigate('/add-edit-employee', { state: { isEditMode: true, employeeData: employeeToEdit } });
  };

  const columns: ColDef<Employee>[] = [
    { field: 'id', headerName: 'Employee ID' },
    { field: 'name', headerName: 'Name' },
    { field: 'emailAddress', headerName: 'Email Address' },
    { field: 'gender', headerName: 'Gender' },
    { field: 'phoneNumber', headerName: 'Phone Number' },
    { field: 'daysWorked', headerName: 'Days Worked' },
    { field: 'cafe', headerName: 'Cafe' },
    {
      headerName: 'Actions',
      cellRenderer: (props: CustomCellRendererProps) => (
        <>
          <Button onClick={() => handleEditEmployee(props.data.id)}>Edit</Button>
          <Button onClick={() => handleDeleteConfirmation(props.data.id)}>Delete</Button>
        </>
      ),
    },
  ];

  return (
    <div>
      <h1>Employees</h1>
      <Card>
        <CardContent>
          <div className="ag-theme-alpine" style={{ height: 400, width: '100%' }}>
            <AgGridReact rowData={employees} columnDefs={columns} />
          </div>

          <Button
            variant="contained"
            color="primary"
            style={{ marginTop: '20px' }}
            onClick={() =>
              navigate('/add-edit-employee', { state: { isEditMode: false, cafeData: {} } })
            }
          >
            Add New Employee
          </Button>
        </CardContent>
      </Card>

      <Dialog open={openDialog} onClose={() => setOpenDialog(false)}>
        <DialogTitle>Confirm Deletion</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete this employee?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpenDialog(false)}>Cancel</Button>
          <Button onClick={handleDeleteEmployee} color="error">
            Delete
          </Button>
        </DialogActions>
      </Dialog>

      <Snackbar
        open={showSnackbar}
        autoHideDuration={3000}
        onClose={() => setShowSnackbar(false)}
        anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
      >
        <Alert onClose={() => setShowSnackbar(false)} severity="success" sx={{ width: '100%' }}>
          Employee deleted successfully!
        </Alert>
      </Snackbar>
    </div>
  );
};

export default Employee;
