import { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { TextField, Button, Snackbar, Alert, CircularProgress, Card, CardContent, Box, Dialog, DialogActions, DialogContent, DialogTitle, Typography } from '@mui/material';
import config from '../config';

interface CafeData {
  name: string;
  description: string;
  location: string;
  logo: string;
}

const AddEditCafe = () => {
  const apiUrl = config.API_BASE_URL;
  const { state } = useLocation();
  const navigate = useNavigate();

  const { isEditMode, cafeData } = state || {};

  const [formData, setFormData] = useState<CafeData>({
    name: '',
    description: '',
    location: '',
    logo: '',
  });
  const [formErrors, setFormErrors] = useState<{ [key: string]: string }>({});
  const [logoError, setLogoError] = useState<string>('');
  const [showSnackbar, setShowSnackbar] = useState<boolean>(false);
  const [snackbarMessage, setSnackbarMessage] = useState<string>('');
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [initialData, setInitialData] = useState<CafeData>({
    name: '',
    description: '',
    location: '',
    logo: '',
  });
  const [openCancelDialog, setOpenCancelDialog] = useState<boolean>(false);

  useEffect(() => {
    if (isEditMode && cafeData) {
      setFormData({
        name: cafeData.name || '',
        description: cafeData.description || '',
        location: cafeData.location || '',
        logo: cafeData.logo || '',
      });
      setInitialData({
        name: cafeData.name || '',
        description: cafeData.description || '',
        location: cafeData.location || '',
        logo: cafeData.logo || '',
      });
    } else {
      setFormData({
        name: '',
        description: '',
        location: '',
        logo: '',
      });
      setInitialData({
        name: '',
        description: '',
        location: '',
        logo: '',
      });
    }
  }, [isEditMode, cafeData]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: value,
    }));
    setFormErrors((prevErrors) => ({ ...prevErrors, [name]: '' }));
  };

  const handleLogoChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files ? e.target.files[0] : null;
    if (!file) return;

    if (file.size > 2 * 1024 * 1024) {
      setLogoError('Logo file size should be less than 2MB');
      return;
    }

    if (file.type !== 'image/png') {
      setLogoError('Only PNG files are allowed');
      return;
    }

    setLogoError('');
    const reader = new FileReader();
    reader.onloadend = () => {
      if (reader.result && typeof reader.result === 'string') {
        const base64Image = reader.result.split(',')[1];
        setFormData((prevState) => ({
          ...prevState,
          logo: base64Image,
        }));
      }
    };
    reader.readAsDataURL(file);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);

    const method = isEditMode ? 'PUT' : 'POST';
    const requestData = isEditMode ? { ...formData, id: cafeData.id } : formData;

    try {
      const response = await fetch(apiUrl + '/Cafe', {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(requestData),
      });

      if (response.ok) {
        setSnackbarMessage(isEditMode ? 'Cafe updated successfully!' : 'Cafe added successfully!');
        setShowSnackbar(true);
        setIsLoading(false);
        setTimeout(() => navigate('/cafes'), 500);
      } else {
        const errorData = await response.json();
        if (errorData?.result?.[0]?.errors) {
          const displayMessage = errorData.result[0].errors[0].displayMessage;
          const validationMessages = displayMessage.split('\r\n');

          const validationErrors = validationMessages.reduce((acc: { [key: string]: string }, message: string) => {
            const [fieldName, ...errorParts] = message.split(' ');
            const errorMessage = errorParts.join(' ');
            acc[fieldName.toLowerCase()] = errorMessage.trim();
            return acc;
          }, {});

          setFormErrors(validationErrors);
        } else {
          throw new Error('Something went wrong');
        }
        setIsLoading(false);
      }
    } catch (error) {
      setSnackbarMessage('Error saving cafe data!');
      setShowSnackbar(true);
      setIsLoading(false);
    }
  };

  const handleCancel = () => {
    if (
      formData.name !== initialData.name ||
      formData.description !== initialData.description ||
      formData.location !== initialData.location ||
      formData.logo !== initialData.logo
    ) {
      setOpenCancelDialog(true);
    } else {
      navigate('/cafes');
    }
  };

  const handleCancelConfirmation = (confirm: boolean) => {
    setOpenCancelDialog(false);
    if (confirm) {
      navigate('/cafes');
    }
  };

  return (
    <Card sx={{ maxWidth: 500, margin: 'auto', padding: 2 }}>
      <CardContent>
        <h1>{isEditMode ? 'Edit Cafe' : 'Add New Cafe'}</h1>
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
            label="Description"
            name="description"
            value={formData.description}
            onChange={handleInputChange}
            error={!!formErrors.description}
            helperText={formErrors.description || ''}
            fullWidth
            required
            sx={{ marginBottom: 2 }}
          />
          <TextField
            label="Location"
            name="location"
            value={formData.location}
            onChange={handleInputChange}
            error={!!formErrors.location}
            helperText={formErrors.location || ''}
            fullWidth
            required
            sx={{ marginBottom: 2 }}
          />
          
          
          <Box sx={{ marginBottom: 2 }}>
            <Typography variant="subtitle1">Logo</Typography>
            {isEditMode && formData.logo && (
              <div style={{ marginBottom: '10px' }}>
                <img
                  src={`data:image/png;base64,${formData.logo}`}
                  alt="Cafe Logo"
                  style={{ width: '100px', height: '100px', objectFit: 'contain', marginBottom: '10px' }}
                />
              </div>
            )}
            <input
              type="file"
              accept="image/png"
              onChange={handleLogoChange}
              style={{ display: 'none' }}
              id="file-upload"
            />
            <label htmlFor="file-upload">
              <Button variant="outlined" component="span">
                Choose File
              </Button>
            </label>
            {logoError && <div style={{ color: 'red' }}>{logoError}</div>}
          </Box>

          <Box sx={{ display: 'flex', justifyContent: 'space-between', marginTop: 2 }}>
            <Button
              type="submit"
              variant="contained"
              color="primary"
              disabled={isLoading}
            >
              {isLoading ? <CircularProgress size={24} /> : 'Submit'}
            </Button>
            <Button variant="outlined" color="secondary" onClick={handleCancel}>
              Cancel
            </Button>
          </Box>
        </form>
      </CardContent>

      <Snackbar
        open={showSnackbar}
        autoHideDuration={3000}
        onClose={() => setShowSnackbar(false)}
        anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
      >
        <Alert onClose={() => setShowSnackbar(false)} severity="success" sx={{ width: '100%' }}>
          {snackbarMessage}
        </Alert>
      </Snackbar>

      
      <Dialog open={openCancelDialog} onClose={() => setOpenCancelDialog(false)}>
        <DialogTitle>Confirm Cancel</DialogTitle>
        <DialogContent>
          <Typography>Are you sure you want to cancel? Your changes will not be saved.</Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => handleCancelConfirmation(false)} color="primary">
            No
          </Button>
          <Button onClick={() => handleCancelConfirmation(true)} color="secondary">
            Yes
          </Button>
        </DialogActions>
      </Dialog>
    </Card>
  );
};

export default AddEditCafe;
