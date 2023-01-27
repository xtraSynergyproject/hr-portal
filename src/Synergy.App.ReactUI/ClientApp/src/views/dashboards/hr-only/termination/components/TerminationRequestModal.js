import React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import TextField from '@mui/material/TextField'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
import MenuBtn from './button_with_modal/MenuBtn'
import { Card } from '@mui/material'
import TerminateBtnModal from './button_with_modal/TerminateBtnModal'

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

function TerminationRequestModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <Box>
      <Button variant='contained' onClick={handleOpen}>
        Termination Request
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
                <Typography sx={{ p: 4 }} variant='h4' component='h3'>
                  Termination Request
                </Typography>

                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignContent: 'center' }}>
                <Button sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }} component='label'>
                  <Icon icon='mdi:attachment-plus' />
                  Attachment
                  <input type='file' hidden />
                </Button>
                <MenuBtn />
                
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

              <Box sx={{ mx: 4}}>
              <Box>
                <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                  <Box sx={{ p: 3 }}>
                    <Typography sx={{ my: 3 }}>
                      Service Owner/Requester: <b>{}</b>
                    </Typography>
                    <Typography sx={{ my: 3 }}>
                      Service Number: <b>{}</b>
                    </Typography>
                    <Typography sx={{ my: 3 }}>
                      Service Status: <b>{}</b>
                    </Typography>
                    <Typography sx={{ my: 3 }}>
                      Due Date: <b>{}</b>
                    </Typography>
                    <Typography sx={{ my: 3 }}>
                      Service Version: <b>{}</b>
                    </Typography>
                  </Box>
                  <Box sx={{ mt: 7 }}>
                    <TextField
                      required
                      fullWidth
                      sx={{ marginBottom: '20px' }}
                      id='date'
                      label='Last Working Date'
                      type='date'
                      defaultValue='YYYY-MM-DD'
                      InputLabelProps={{
                        shrink: true
                      }}
                    />

                    <TextField
                      fullWidth
                      required
                      id='date'
                      label='Resignation/Termination Date'
                      type='date'
                      defaultValue='YYYY-MM-DD'
                      InputLabelProps={{
                        shrink: true
                      }}
                    />
                  </Box>
                </Box >
                <TextField id='outlined-basic' fullWidth label='Reason' variant='outlined' sx={{mb : 3}}/>
                <TextField id='outlined-basic' fullWidth label='Comment' multiline rows={4} variant='outlined' />
              </Box>

              <Box sx={{ display: 'flex', flexWrap: 'wrap', justifyContent: 'space-between', alignContent: 'center' }}>
                <Card sx={{ width: '450px', borderBlockWidth: '1px', borderBlockStyle: 'solid', my: 3 }}>
                  <Typography sx={{ m: 2 }}>
                    <b>Line Manager</b>{' '}
                  </Typography>

                  <hr />
                  <Typography sx={{ m: 20 }}>{}</Typography>
                </Card>

                <Card sx={{ width: '450px', borderBlockWidth: '1px', borderBlockStyle: 'solid', my: 3 }}>
                  <Typography sx={{ m: 2 }}>
                    <b>Department Manager</b>{' '}
                  </Typography>
                  <hr /> <Typography sx={{ m: 20 }}>{}</Typography>
                </Card>

                <Card sx={{ width: '450px', borderBlockWidth: '1px', borderBlockStyle: 'solid', my: 3 }}>
                  <Typography sx={{ m: 2 }}>
                    <b>Finance Department</b>{' '}
                  </Typography>
                  <hr /> <Typography sx={{ m: 20 }}>{}</Typography>
                </Card>

                <Card sx={{ width: '450px', borderBlockWidth: '1px', borderBlockStyle: 'solid', my: 3 }}>
                  <Typography sx={{ m: 2 }}>
                    <b>HR Department</b>{' '}
                  </Typography>
                  <hr /> <Typography sx={{ m: 20 }}>{}</Typography>
                </Card>
              </Box>
              <Box sx={{ display: 'flex', my: 5, justifyContent: 'flex-end', gap: 3 }}>
                <TerminateBtnModal />
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

export default TerminationRequestModal
