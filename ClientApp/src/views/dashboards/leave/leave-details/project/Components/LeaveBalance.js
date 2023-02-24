
import * as React from 'react';
import PropTypes from 'prop-types';
import Button from '@mui/material/Button';
import { styled } from '@mui/material/styles';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import Typography from '@mui/material/Typography';
import Box from "@mui/material/Box";
import Grid from '@mui/material/Unstable_Grid2';
import TextField from "@mui/material/TextField";




const BootstrapDialog = styled(Dialog)(({ theme }) => ({
  '& .MuiDialogContent-root': {
    padding: theme.spacing(2),
  },
  '& .MuiDialogActions-root': {
    padding: theme.spacing(1),
  },
}));





function BootstrapDialogTitle(props) {
  const { children, onClose, ...other } = props;

  return (
    <DialogTitle sx={{ m: 0, p: 2 }} {...other}>
      {children}
      {onClose ? (
        <IconButton
          aria-label="close"
          onClick={onClose}
          sx={{
            position: 'absolute',
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <CloseIcon />
        </IconButton>
      ) : null}
    </DialogTitle>
  );
}

BootstrapDialogTitle.propTypes = {
  children: PropTypes.node,
  onClose: PropTypes.func.isRequired,
};

export default function CustomizedDialogs() {
  const [open, setOpen] = React.useState(false);

  const handleClickOpen = () => {
    setOpen(true);
  };
  const handleClose = () => {
    setOpen(false);
  };

  return (
    <div>
      <Button size="small" sx={{ ml: 1, color: 'text.secondary', fontWeight: 600, }} onClick={handleClickOpen}>Annual Leave Balance Projections</Button>

      <BootstrapDialog
        onClose={handleClose}
        aria-labelledby="customized-dialog-title"
        open={open}
        maxWidth={'lg'}
      >
        <BootstrapDialogTitle id="customized-dialog-title" onClose={handleClose}>
          Annual Leave Balance Projections
        </BootstrapDialogTitle>
        <DialogContent dividers >
          <Box sx={{ height: 300, width: 600, background: 'white', transform: 'translateZ(0px)', flexGrow: 1 }}>

            <Grid item xs={6}>

              <TextField
                fullWidth
                sx={{ marginBottom: "8px" }}
                id="date"
                label="Date Of Birth"
                type="date"
                defaultValue="YYYY-MM-DD"
                InputLabelProps={{
                  shrink: true,
                }}
              />
              <Button variant="contained">Get Balance</Button>

            </Grid>

            <Typography variant="h6" sx={{ display: 'flex', justifyContent: 'center' }}>LeaveBalance</Typography>
            <Typography sx={{ display: 'flex', justifyContent: 'center' }}></Typography>
          </Box>





        </DialogContent>
        <DialogActions>
          {/* <Button autoFocus onClick={handleClose}>
            Save changes
          </Button> */}
        </DialogActions>
      </BootstrapDialog>

    </div>
  );
}
