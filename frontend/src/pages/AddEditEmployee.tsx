import { useState, useEffect, ChangeEvent } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { TextField, Button, Snackbar, Alert, CircularProgress, Card, CardContent, Box, Dialog, DialogActions, DialogContent, DialogTitle, Typography, Radio, RadioGroup, FormControlLabel, InputLabel, MenuItem, FormControl, Select, SelectChangeEvent } from '@mui/material';
import config from '../config';

interface EmployeeFormData {
  id: string;
  name: string;
  emailAddress: string;
  gender: string;
  phoneNumber: string;
  cafe: string;
}

interface EmployeeFormErrors {
  name?: string;
  emailAddress?: string;
  gender?: string;
  phoneNumber?: string;
  cafe?: string;
}

const AddEditEmployee = () => {
  const apiUrl = config.API_BASE_URL;
  const { state } = useLocation();
  const navigate = useNavigate();

  const { isEditMode, employeeData } = state || {};

  const [formData, setFormData] = useState<EmployeeFormData>({
    id: '',
    name: '',
    emailAddress: '',
    gender: '',
    phoneNumber: '',
    cafe: '',
  });
  const [formErrors, setFormErrors] = useState<EmployeeFormErrors>({});
  const [showSnackbar, setShowSnackbar] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [cafes, setCafes] = useState<any[]>([]);
  const [initialData, setInitialData] = useState<EmployeeFormData>({} as EmployeeFormData);
  const [openCancelDialog, setOpenCancelDialog] = useState(false);

  const fetchCafes = async () => {
    setIsLoading(true);

    try {
      const response = await fetch(apiUrl + '/Cafe');
      const data = await response.json();
      if (data.success && Array.isArray(data.result)) {
        setCafes(data.result[0].payload);
      }
    } catch (error) {
      console.error('Error fetching cafes:', error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchCafes();

    if (isEditMode && employeeData) {
      setFormData({
        id: employeeData.id || '',
        name: employeeData.name || '',
        emailAddress: employeeData.emailAddress || '',
        gender: employeeData.gender || '',
        phoneNumber: employeeData.phoneNumber || '',
        cafe: employeeData.cafe || '',
      });
      setInitialData({ ...employeeData });
    } else {
      setFormData({
        id: '',
        name: '',
        emailAddress: '',
        gender: '',
        phoneNumber: '',
        cafe: '',
      });
      setInitialData({} as EmployeeFormData);
    }
  }, [isEditMode, employeeData]);

  const handleInputChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement | { name?: string; value: unknown }>) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({
      ...prevState,
      [name as string]: value,
    }));
    setFormErrors((prevErrors) => ({ ...prevErrors, [name as string]: '' }));
  };

  const handleSelectChange = (e: SelectChangeEvent<string>) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: value,
    }));
    setFormErrors((prevErrors) => ({ ...prevErrors, [name]: '' }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);

    const method = isEditMode ? 'PUT' : 'POST';
    const requestData = isEditMode ? { ...formData, id: employeeData.id } : formData;

    try {
      const response = await fetch(apiUrl + '/Employee', {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(requestData),
      });

      if (response.ok) {
        setSnackbarMessage(isEditMode ? 'Employee updated successfully!' : 'Employee added successfully!');
        setShowSnackbar(true);
        setTimeout(() => navigate('/employees'), 500);
      } else {
        const errorData = await response.json();
        if (errorData?.result?.[0]?.errors) {
          const displayMessage = errorData.result[0].errors[0].displayMessage;
          const validationMessages = displayMessage.split('\r\n');

          const validationErrors = validationMessages.reduce(
            (acc: EmployeeFormErrors, message: string) => {
              const [fieldName, ...errorParts] = message.split(' ');
              const errorMessage = errorParts.join(' ');
              acc[fieldName.toLowerCase() as keyof EmployeeFormErrors] = errorMessage.trim();
              return acc;
            },
            {} as EmployeeFormErrors
          );

          setFormErrors(validationErrors);
        } else {
          throw new Error('Something went wrong');
        }
      }
    } catch (error) {
      setSnackbarMessage('Error saving employee data!');
      setShowSnackbar(true);
    } finally {
      setIsLoading(false);
    }
  };

  const handleCancel = () => {
    if (
      formData.name !== initialData.name ||
      formData.emailAddress !== initialData.emailAddress ||
      formData.gender !== initialData.gender ||
      formData.phoneNumber !== initialData.phoneNumber ||
      formData.cafe !== initialData.cafe
    ) {
      setOpenCancelDialog(true);
    } else {
      navigate('/employees');
    }
  };

  const handleCancelConfirmation = (confirm: boolean) => {
    setOpenCancelDialog(false);
    if (confirm) {
      navigate('/employees');
    }
  };

  return (
    <Card sx={{ maxWidth: 500, margin: 'auto', padding: 2 }}>
      <CardContent>
        <h1>{isEditMode ? 'Edit Employee' : 'Add New Employee'}</h1>
        <form onSubmit={handleSubmit}>
          <TextField
            label="Name"
            name="name"
            value={formData.name}
            onChange={handleInputChange}
            error={!!formErrors.name}
            helperText={formErrors.name || ''}
            fullWidth
            required
            sx={{ marginBottom: 2 }}
          />

          <TextField
            label="Email Address"
            name="emailAddress"
            value={formData.emailAddress}
            onChange={handleInputChange}
            error={!!formErrors.emailAddress}
            helperText={formErrors.emailAddress || ''}
            fullWidth
            required
            sx={{ marginBottom: 2 }}
          />

          <Box sx={{ marginBottom: 2 }}>
            <Typography variant="subtitle1">Gender</Typography>
            <RadioGroup row name="gender" value={formData.gender} onChange={handleInputChange}>
              <FormControlLabel value="Male" control={<Radio />} label="Male" />
              <FormControlLabel value="Female" control={<Radio />} label="Female" />
            </RadioGroup>
            {formErrors.gender && <div style={{ color: 'red' }}>{formErrors.gender}</div>}
          </Box>

          <TextField
            label="Phone Number"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleInputChange}
            error={!!formErrors.phoneNumber}
            helperText={formErrors.phoneNumber || ''}
            fullWidth
            required
            sx={{ marginBottom: 2 }}
          />

          <FormControl fullWidth sx={{ marginBottom: 2 }}>
            <InputLabel>Cafe</InputLabel>
            <Select
              label="Cafe"
              name="cafe"
              value={formData.cafe}
              onChange={handleSelectChange}
              error={!!formErrors.cafe}
            >
              {cafes?.map((cafe) => (
                <MenuItem key={cafe.name} value={cafe.name}>
                  {cafe.name}
                </MenuItem>
              ))}
            </Select>
            {formErrors.cafe && <div style={{ color: 'red' }}>{formErrors.cafe}</div>}
          </FormControl>

          <Box sx={{ display: 'flex', justifyContent: 'space-between', marginTop: 2 }}>
            <Button
              type="submit"
              variant="contained"
              color="primary"
              disabled={isLoading}
              sx={{ width: '48%' }}
            >
              {isLoading ? <CircularProgress size={24} /> : isEditMode ? 'Update Employee' : 'Add Employee'}
            </Button>
            <Button variant="outlined" color="secondary" onClick={handleCancel} sx={{ width: '48%' }}>
              Cancel
            </Button>
          </Box>
        </form>

        <Dialog open={openCancelDialog} onClose={() => setOpenCancelDialog(false)}>
          <DialogTitle>Unsaved Changes</DialogTitle>
          <DialogContent>
            <Typography>Are you sure you want to cancel? Your changes will be lost.</Typography>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => handleCancelConfirmation(false)} color="primary">
              No
            </Button>
            <Button onClick={() => handleCancelConfirmation(true)} color="primary">
              Yes
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
            {snackbarMessage}
          </Alert>
        </Snackbar>
      </CardContent>
    </Card>
  );
};

export default AddEditEmployee;
