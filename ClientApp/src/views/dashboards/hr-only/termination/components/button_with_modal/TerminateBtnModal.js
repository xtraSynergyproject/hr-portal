import React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import TextField from '@mui/material/TextField'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
import VersionModal from './VersionModal'
import TagsModal from './TagsModal'
import LogModal from './LogModal'

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
  margin: "auto",
//   marginTop: 'auto',
//   marginBottom: 'auto',
//   marginX: '280px'
}

const modalContentStyle = {
  position: 'relative',
  background: '#fff',
  boxShadow: 24,
  mt: 3,
  width: '50rem',
  mb: 3,
  borderRadius: '10px'
}

export default function TerminateBtnModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <div>
      <Button onClick={handleOpen} variant='contained'>
        Terminate Employee
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
            <Box sx={{ mb: 4, display: 'flex', justifyContent: 'space-between' }} className='demo-space-x'>
              <Typography sx={{ p: 4 }} variant='h5' component='h3'>
                Terminate Employee
              </Typography>

              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignContent: 'center', alignItems:"center" }}>
                
                <VersionModal/>

                <Button sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }} component='label'>
                  <Icon icon='mdi:attachment-plus' />{' '}
         Attachment
                  <input type='file' hidden />
                </Button>

               <TagsModal/>

               <LogModal/>

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
            <hr />
            <Box sx={{m:4}}>
            <Typography sx={{ my: 3 }}>
                    Note Number :<b>{}</b>
                  </Typography>

            <Typography sx={{ my: 3 }}>
                    Status :<b>{}</b>
                  </Typography>

            <TextField id='outlined-basic' fullWidth label='Reason' variant='outlined' sx={{my : 3}}/>

            <TextField
                      fullWidth
                      sx={{mt:3}}
                      required
                      id='date'
                      label='Last Working Date'
                      type='date'
                      defaultValue='YYYY-MM-DD'
                      InputLabelProps={{
                        shrink: true
                      }}
                    />
          </Box>
          </Box>
        </Box>
      </Modal>
    </div>
  )
}
