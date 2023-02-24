import * as React from 'react'
import Box from '@mui/material/Box'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
import ShareIcon from '@mui/icons-material/Share'
import DeleteIcon from '@mui/icons-material/Delete';
import ShareTab from './ShareTab'
import { styled } from '@mui/material/styles'
import ShareRoundedIcon from '@mui/icons-material/ShareRounded';

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
  width: '40rem',
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

export default function NoteShareModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (

    <div>
      <ShareIcon onClick={handleOpen} />
        
     
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
                Share
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

            <Box>
              <Box>
                <ShareTab />
              </Box>

              <Box sx={{height:"50px", display:"flex", justifyContent:"space-between", alignItems:"center", px:"10px"}}>
                <ProfilePicture
                  //   src={data.PhotoName}
                  src={''}
                  alt='profile-picture'
                  sx={{ width: '150px', height: '150px', border: '5px solid #f0f0f0' }}
                />

                <Typography>User Name</Typography>
                <Typography>User/Team</Typography>
                <Typography>< DeleteIcon/></Typography>

              </Box>
            </Box>
          </Box>
        </Box>
      </Modal>
    </div>
  )
}
