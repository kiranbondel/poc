import { useState, useEffect } from 'react';
import { AgGridReact, CustomCellRendererProps } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { Button, Select, MenuItem, FormControl, InputLabel, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Snackbar, Alert, Card, CardContent } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import config from '../config';
import { ColDef } from 'ag-grid-community';

interface Cafe {
  id: string;
  logo: string;
  name: string;
  description: string;
  location: string;
  noOfEmployees: number;
}

const CafePage = () => {
  const apiUrl = config.API_BASE_URL;
  const [cafes, setCafes] = useState<Cafe[]>([]);
  const [filteredCafes, setFilteredCafes] = useState<Cafe[]>([]);
  const [locationFilter, setLocationFilter] = useState<string>('');
  const [allLocations, setAllLocations] = useState<string[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [deleteId, setDeleteId] = useState<string>('');
  const [showSnackbar, setShowSnackbar] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    fetchCafes();
  }, []);

  const LogoRenderer = (props: CustomCellRendererProps) => (
    <span
      style={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        height: '100%',
      }}
    >
      {props.value ? (
        <img
          src={`data:image/png;base64,${props.value}`}
          alt="Logo"
          style={{
            width: '50px',
            height: '50px',
            objectFit: 'contain',
          }}
        />
      ) : (
        <span>No Logo</span>
      )}
    </span>
  );

  const fetchCafes = async (location = '') => {
    const url = location
      ? `${apiUrl}/Cafe?location=${encodeURIComponent(location)}`
      : `${apiUrl}/Cafe`;

    try {
      const response = await fetch(url);
      const data = await response.json();
      if (data.success && Array.isArray(data.result)) {
        const cafes = data.result[0].payload;
        setCafes(cafes);
        setFilteredCafes(cafes);

        if (location === '') {
          const locations: string[] = Array.from(new Set(cafes.map((cafe: Cafe) => cafe.location)));
          setAllLocations(locations);
        } else if (Array.isArray(cafes) && cafes.length === 0) {
          setAllLocations(allLocations.filter((loc) => loc !== location));
        }
      }
    } catch (error) {
      console.error('Error fetching cafes:', error);
    }
  };

  const handleDeleteConfirmation = (id: string) => {
    setDeleteId(id);
    setOpenDialog(true);
  };

  const handleDeleteCafe = async () => {
    try {
      const url = `${apiUrl}/Cafe?Id=${encodeURIComponent(deleteId)}`;
      const response = await fetch(url, {
        method: 'DELETE',
      });

      if (response.ok) {
        setOpenDialog(false);
        setShowSnackbar(true);
        fetchCafes();
      } else {
        console.error('Error deleting cafe');
      }
    } catch (error) {
      console.error('Error deleting cafe:', error);
    }
  };

  const handleEditCafe = (id: string) => {
    const cafeToEdit = cafes.find((cafe) => cafe.id === id);
    navigate('/add-edit-cafe', { state: { isEditMode: true, cafeData: cafeToEdit } });
  };

  const handleFilterChange = (location: string) => {
    setLocationFilter(location);
    fetchCafes(location === 'All Locations' ? '' : location);
  };

  const goToEmployeesPage = (cafeName: string) => {
    navigate(`/employees?cafe=${encodeURIComponent(cafeName)}`);
  };

  const columns: ColDef<Cafe>[] = [
    { field: 'logo', headerName: 'Logo', cellRenderer: LogoRenderer },
    { field: 'name', headerName: 'Name' },
    { field: 'description', headerName: 'Description' },
    { field: 'location', headerName: 'Location' },
    {
      field: 'noOfEmployees',
      headerName: 'Employees',
      cellRenderer: (props: { data: Cafe; value: number }) => (
        <Button onClick={() => goToEmployeesPage(props.data.name)}>
          {props.value}
        </Button>
      ),
    },
    {
      headerName: 'Actions',
      cellRenderer: (props: CustomCellRendererProps) => (
        <>
          <Button onClick={() => handleEditCafe(props.data.id)}>Edit</Button>
          <Button onClick={() => handleDeleteConfirmation(props.data.id)}>Delete</Button>
        </>
      ),
    },
  ];

  return (
    <div>
      <h1>Cafe List</h1>
      <div style={{ display: 'flex', alignItems: 'center', marginBottom: '20px' }}>
        <FormControl style={{ minWidth: '200px', marginRight: '20px' }}>
          <InputLabel>Select Location</InputLabel>
          <Select
            value={locationFilter}
            onChange={(e) => handleFilterChange(e.target.value)}
            label="Select Location"
          >
            <MenuItem value="All Locations">All Locations</MenuItem>
            {allLocations.map((loc) => (
              <MenuItem key={loc} value={loc}>
                {loc}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </div>

      <Card>
        <CardContent>
          <div className="ag-theme-alpine" style={{ height: 400, marginBottom: '20px' }}>
            <AgGridReact rowData={filteredCafes} columnDefs={columns} />
          </div>
          <Button
            variant="contained"
            color="primary"
            style={{ marginTop: '20px' }}
            onClick={() =>
              navigate('/add-edit-cafe', { state: { isEditMode: false, cafeData: {} } })
            }
          >
            Add New Cafe
          </Button>
        </CardContent>
      </Card>

      <Dialog open={openDialog} onClose={() => setOpenDialog(false)}>
        <DialogTitle>Confirm Delete</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete this cafe?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpenDialog(false)} color="primary">
            Cancel
          </Button>
          <Button onClick={handleDeleteCafe} color="secondary">
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
        <Alert onClose={() => setShowSnackbar(false)} severity="success">
          Cafe deleted successfully!
        </Alert>
      </Snackbar>
    </div>
  );
};

export default CafePage;
