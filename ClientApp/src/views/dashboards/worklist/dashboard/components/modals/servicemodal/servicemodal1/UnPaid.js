import React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import TextField from '@mui/material/TextField'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
// import MenuBtn from './MenuBtn'
import { Card, Divider } from '@mui/material'
import Select from '@mui/material/Select'
import MenuItem from '@mui/material/MenuItem'
import InputLabel from '@mui/material/InputLabel'
import FormControl from '@mui/material/FormControl'
import Avatar from '@mui/material/Avatar';
import Fab from '@mui/material/Fab';
import EditIcon from '@mui/icons-material/Edit';
import { AttachFile } from '@mui/icons-material';
import CloseIcon from '@mui/icons-material/Close';
import MoreHorizIcon from '@mui/icons-material/MoreHoriz';
import EmailIcon from '@mui/icons-material/Email';



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

function MisconductsModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <Box>
      <Button variant='contained' size='medium' sx={{ width: '5rem' }} onClick={handleOpen}>
        Create
      </Button>

      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby='modal-modal-title'
        aria-describedby='modal-modal-description'
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>
            <Box sx={{ mb: 2, display: 'flex', justifyContent: 'space-between' }} className='demo-space-x'>
              <Typography sx={{ p: 4 }} variant='h5' component='h3'>
                CreateService
              </Typography>

              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignContent: 'center' }}>
                {/*   <Button sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }} component='label'>
                  <Icon icon='mdi:attachment-plus' />
                  Attachment
                  <input type='file' hidden />
                </Button>

                <MenuBtn /> */}

                <Button
                  sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }}
                  onClick={handleClose}
                  component='label'
                >
                  <Icon icon='mdi:close' />
                  Close
                </Button>
              </Box>
            </Box>
            <Divider />
            <Box sx={{ display: 'column' }}>
              <Box
                sx={{

                  ml: 10,
                  width: '15rem',
                  height: '7vh',
                  backgroundColor: '#346cb0',
                  borderRadius: 10,
                }}

              >
                <Box sx={{ display: 'flex', ml: 7 }}>
                  <Avatar alt="Travis Howard" image="https://media.istockphoto.com/id/1368424494/photo/studio-portrait-of-a-cheerful-woman.jpg?s=1024x1024&w=is&k=20&c=9eszAhNKMRzMHVp4wlmFRak8YyH3rAU8re9HjKA6h3A=" />
                  <Typography sx={{ ml: 5, color: '#fff' }}>UnAbsent Laeave</Typography>
                </Box>

              </Box>

              <Box sx={{display:'flex'}}>

                <Typography variant='h6' component='p' sx={{ mt: 10, mb: 3 }}>
                  UnAbsent Leave
                </Typography>
                <Box sx={{ '& > :not(style)': { m: 1 } }}>
                  <Fab size="small" color="primary" aria-label="edit">
                    <EditIcon />
                  </Fab>
                  <Fab size="small" color="secondary" aria-label="attach">
                    <AttachFile />
                  </Fab>

                  <Fab size="small" color="secondary" aria-label="email">
                    <EmailIcon />
                  </Fab>

                  <Fab size="small" disabled aria-label="more">
                    <MoreHorizIcon />
                  </Fab>

                  <Fab size="small" color="secondary" aria-label="close">
                    <CloseIcon />
                  </Fab>


                </Box>
                <TextField
                  id='outlined-basic'
                  fullWidth
                  label='Investigation Result'
                  multiline
                  rows={3}
                  variant='outlined'
                  sx={{ mt: 3, mb: 1.5 }}
                />
                <TextField
                  id='outlined-basic'
                  fullWidth
                  label='Desription'
                  multiline
                  rows={4}
                  variant='outlined'
                  sx={{ mt: 1.5, mb: 5 }}
                />
              </Box>

              <Box sx={{ display: 'flex', my: 5, justifyContent: 'flex-end', gap: 3 }}>
                {/* <MoreDetailsModal/> */}
                <Button variant='contained'>Save As Draft</Button>
                <Button variant='contained'>Submit</Button>
              </Box>
            </Box>
          </Box>
        </Box>
      </Modal>
    </Box>
  )
}

export default MisconductsModal
