import * as React from 'react'
import Box from '@mui/material/Box'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
import FormControlLabel from '@mui/material/FormControlLabel'
import Checkbox from '@mui/material/Checkbox'
import NotificationsIcon from '@mui/icons-material/Notifications'
import { styled, alpha } from '@mui/material/styles'
import InputBase from '@mui/material/InputBase'
import SearchIcon from '@mui/icons-material/Search'
import DeleteIcon from '@mui/icons-material/Delete'
import DoneAllIcon from '@mui/icons-material/DoneAll'

const Search = styled('div')(({ theme }) => ({
  position: 'relative',
  borderRadius: theme.shape.borderRadius,
  backgroundColor: alpha(theme.palette.common.white, 0.15),
  '&:hover': {
    backgroundColor: alpha(theme.palette.common.white, 0.25)
  },
  marginLeft: 0,
  width: '100%',
  [theme.breakpoints.up('sm')]: {
    marginLeft: theme.spacing(1),
    width: 'auto'
  }
}))

const SearchIconWrapper = styled('div')(({ theme }) => ({
  padding: theme.spacing(0, 2),
  height: '100%',
  position: 'absolute',
  pointerEvents: 'none',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center'
}))

const StyledInputBase = styled(InputBase)(({ theme }) => ({
  color: 'inherit',
  '& .MuiInputBase-input': {
    padding: theme.spacing(1, 1, 1, 0),
    // vertical padding + font size from searchIcon
    paddingLeft: `calc(1em + ${theme.spacing(4)})`,
    transition: theme.transitions.create('width'),
    width: '100%',
    [theme.breakpoints.up('sm')]: {
      width: '50ch',
      '&:focus': {
        width: '50ch'
      }
    }
  }
}))

const modalWrapper = {
  overflow: 'auto',
  maxHeight: '100vh',
  display: 'flex'
}

const modalBlock = {
  position: 'relative',
  zIndex: 0,
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  margin: 'auto'
}

const modalContentStyle = {
  position: 'relative',
  background: '#fff',
  boxShadow: 24,
  mt: 3,
  width: '60rem',
  mb: 3,
  borderRadius: '10px'
}

const ProfilePicture = styled('img')(({ theme }) => ({
  width: 120,
  height: 120,
  borderRadius: theme.shape.borderRadius,
  border: `5px solid ${theme.palette.common.white}`,
  [theme.breakpoints.down('md')]: {
    marginBottom: theme.spacing(4)
  }
}))

export default function NoteNotificationModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <div>
      <Box
        onClick={handleOpen}
        sx={{
          flex: 0.75,
          mx: '5px',
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          cursor: 'pointer'
        }}
      >
        <small>Notifications</small>
        <NotificationsIcon sx={{ color: 'lightslategray' }} fontSize='large' />
      </Box>

      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby='modal-modal-title'
        aria-describedby='modal-modal-description'
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>
            <Box
              sx={{ display: 'flex', justifyContent: 'space-between', alignContent: 'center', alignItems: 'center' }}
            >
              <Typography sx={{ p: 5 }} variant='h5' component='h3'>
                Notifications
              </Typography>

              <Typography
                sx={{
                  mx: '10px',
                  borderRadius: '50px',
                  display: 'flex',
                  flexDirection: 'column',
                  alignItems: 'center',
                  fontSize: '13px',
                  cursor: 'pointer'
                }}
                onClick={handleClose}
                component='label'
              >
                <Icon icon='mdi:close' fontSize='18px' />
                Close
              </Typography>
            </Box>
            <hr />
            <Box sx={{ px: '10px' }}>
              <Box sx={{ px: '5px', display: 'flex', justifyContent: 'space-between', width:"60rem" }}>
                <Box sx={{ px: '5px', display: 'flex', justifyContent: 'space-between' }}>
                  <FormControlLabel control={<Checkbox />} label='Exclude Read' />
                  <FormControlLabel control={<Checkbox />} label='Include Archive' />
                  <FormControlLabel control={<Checkbox />} label='Include Completed' />
                </Box>
                <Search sx={{ display: 'flex', alignItems: 'center' }}>
                  <SearchIconWrapper>
                    <SearchIcon />
                  </SearchIconWrapper>
                  <StyledInputBase
                    placeholder='Search by Reference, Subject, Description and Form'
                    inputProps={{ 'aria-label': 'search' }}
                  />
                </Search>
              </Box>

              <Box>
                <Box sx={{ display: 'flex',p:"8px", my:"20px" , border:'1px #D3D3D3 solid',borderRadius: '7px'
}}>
                  <ProfilePicture
                    //   src={data.PhotoName}
                    src={''}
                    alt='profile-picture'
                    sx={{ width: '70px', height: '70px', border: '5px solid #f0f0f0', mr: '20px' }}
                  />
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', width: '60rem' }}>
                    <Box>
                      <tr>
                        <th></th>

                        <th></th>
                      </tr>
                      <tr>
                        <td>Reference: </td>
                        <td> <b></b> </td>
                      </tr>
                      <tr>
                        <td>From: </td>
                        <td> <b></b> </td>
                      </tr>
                      <tr>
                        <td>Subject: </td>
                        <td> <b></b> </td>
                      </tr>
                    </Box>
                    <Box>
                      <tr>
                        <td>Date: </td>
                        <td> <b></b> </td>
                      </tr>
                      <tr>
                        <td>Description:</td>
                        <td> <b></b> </td>
                      </tr>
                    </Box>
                    <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                      <DeleteIcon sx={{ my: '5px', cursor: 'pointer' }} />
                      <DoneAllIcon sx={{ my: '5px', cursor: 'pointer' }} />
                    </Box>
                  </Box>
                </Box>
              </Box>
            </Box>
          </Box>
        </Box>
      </Modal>
    </div>
  )
}
