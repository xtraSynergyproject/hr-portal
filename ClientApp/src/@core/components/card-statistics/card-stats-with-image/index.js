// ** MUI Imports
import React from 'react'
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import { styled } from '@mui/material/styles'
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import PropTypes from 'prop-types';
import Button from "@mui/material/Button";
import Modal from "@mui/material/Modal";
import Container from '@mui/material/Container';

// ** Custom Components Imports
import CustomChip from 'src/@core/components/mui/chip'

import LeaveType from '../../../../views/dashboards/leave/leave-details/project/Components/LeaveType'
import TimePermissionBox from '../../../../views/dashboards/leave/time-permison/Project3/Components/TimePermissonBox'
import EmployeeProfile from 'src/pages/dashboards/employee-profile/index'
import EmployeeDirectory from 'src/views/dashboards/employee-dashboard/EmployeeDirectory'

// ** Styled component for the image
const Img = styled('img')({
  right: 7,
  bottom: 0,
  height: 177,
  position: 'absolute'
})

//** Model */
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

//

const CardStatsCharacter = ({ data }) => {
  // ** Vars
  const { title, chipText, src, stats, trendNumber, trend = 'positive', chipColor = 'primary' } = data

  //** Model */
  const [open, setOpen] = React.useState(false);

  const handleClickOpen = () => {
    setOpen(true);
  };
  const handleClose = () => {
    setOpen(false);
  };
  //
  console.log("data cc", data);
  return (
    <Card sx={{ overflow: 'visible', position: 'relative' }}>
      <CardContent>
        <Typography sx={{ mb: 6.5, fontWeight: 600 }}>{title}</Typography>
        <Box sx={{ mb: 1.5, rowGap: 1, width: '55%', display: 'flex', flexWrap: 'wrap', alignItems: 'flex-start' }}>
          <Typography variant='h5' sx={{ mr: 1.5 }}>
            {stats}
          </Typography>
          <Typography
            component='sup'
            variant='caption'
            sx={{ color: trend === 'negative' ? 'error.main' : 'success.main' }}
          >
            {trendNumber}
          </Typography>
        </Box>
         <div>
          <Button  onClick={handleClickOpen}>
          {chipText}
          </Button>
          <BootstrapDialog
            onClose={handleClose}
            aria-labelledby="customized-dialog-title"
            open={open}
            maxWidth='lg'
          >
            <BootstrapDialogTitle id="customized-dialog-title" onClose={handleClose}>
              Modal title
            </BootstrapDialogTitle>
            <DialogContent dividers>
              <Container sx={{ padding: '5px' }} >

                <Box sx={{ display: 'flex', backgroundColor: ' #F4F5FA' }}>
                  {
                    data.id === 1 ? <LeaveType /> : null
                  }
                  {
                    data.id === 2 ? <TimePermissionBox /> : null
                  }
                  {
                    data.id === 3 ? <EmployeeDirectory /> : null
                  }
                  {
                    data.id === 4 ? <EmployeeProfile /> : null
                  }
                </Box>
              </Container>
            </DialogContent>
            <DialogActions>
              {/* <Button autoFocus onClick={handleClose}>
                Save changes
              </Button> */}
            </DialogActions>
          </BootstrapDialog>
        </div>

        <Img src={src} alt={title} />

      </CardContent>
    </Card>
  )
}

export default CardStatsCharacter
