import * as React from 'react'

import Box from '@mui/material/Box'

import Button from '@mui/material/Button'

import Typography from '@mui/material/Typography'
import Icon from 'src/@core/components/icon'

import Modal from '@mui/material/Modal'

import TextField from '@mui/material/TextField'
import MenuBtn from './MenuBtn'

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

  width: '50rem',

  mb: 3,

  borderRadius: '10px'
}

export default function MedicsalReimbModal() {
  const [open, setOpen] = React.useState(false)

  const handleOpen = () => setOpen(true)

  const handleClose = () => setOpen(false)

  return (
    <div>
      <Button variant='contained' onClick={handleOpen}>
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
              <Typography sx={{ p: 4 }} variant='h4' component='h3'>
                Create Request
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
            <Box sx={{ py: 3, px: 5 }}>
              <Box>
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
              <Box>
                <TextField
                  required
                  fullWidth
                  sx={{ marginY: '5px' }}
                  id='date'
                  label='Date'
                  type='date'
                  defaultValue='YYYY-MM-DD'
                  InputLabelProps={{
                    shrink: true
                  }}
                />

                <TextField
                  sx={{ marginY: '5px' }}
                  fullWidth
                  id='outlined-multiline-static'
                  label='Detals'
                  multiline
                  rows={4}
                />
                <TextField
                  sx={{ marginY: '5px' }}
                  required
                  fullWidth
                  id='outlined-basic'
                  label='Reimbursement Amount'
                  variant='outlined'
                />

                <Box
                  sx={{
                    mt: 6
                  }}
                >
                  <Typography variant='outlined'>
                    Supporting Document :
                    <Button component='label'>
                      Browse Document
                      <input type='file' hidden />
                    </Button>
                  </Typography>
                  <Box sx={{width:"700px",display:"flex", justifyContent:"space-between",}}> 
                    <Box ><Typography>File Name : {} </Typography></Box>
                    <Box ><Typography>File Size : {} </Typography></Box>
                  </Box>
                </Box>
              </Box>
              <Box sx={{ display: 'flex',mt:15, mb: 5, justifyContent: 'flex-end', gap: 3 }}>
                <Button variant='contained'>Save As Draft</Button>
                <Button variant='contained'>Submit</Button>
              </Box>
            </Box>
          </Box>
        </Box>
      </Modal>
    </div>
  )
}
