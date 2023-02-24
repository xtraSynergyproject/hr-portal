import * as React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import Icon from 'src/@core/components/icon'
import Modal from '@mui/material/Modal'
import TextField from '@mui/material/TextField'
import Divider from '@mui/material/Divider'
import MenuBtn from 'src/views/dashboards/hr-only/termination/components/button_with_modal/MenuBtn'
import ModalUserInfo from 'src/views/dashboards/hr-only/termination/ModalUserInfo'

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

export default function TravelReimbModal() {
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
          <Box sx={{ mb: "10px", display: 'flex', justifyContent: 'space-between', alignItems:"center" }} className='demo-space-x'>
              <Typography sx={{ pl: 4 }} variant='h5' component='h3'>
                Create Request
              </Typography>

              <Box sx={{width:"200px",height:"60px", display: 'flex', justifyContent: 'space-between', alignContent: 'center' }}>
                <Typography
                  sx={{
                    borderRadius: '50px',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    fontSize: '13px',
                    cursor:"pointer"
                  }}
                  component='label'
                >
                  <Icon icon='mdi:attachment-plus' fontSize='18px' />
                  Attachment
                  <input type='file' hidden />
                </Typography>
                <MenuBtn />

                <Typography
                  sx={{
                    borderRadius: '50px',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    fontSize: '13px',
                    cursor:"pointer"
                  }}
                  onClick={handleClose}
                  component='label'
                >
                  <Icon icon='mdi:close' fontSize='18px' />
                  Close
                </Typography>
              </Box>
            </Box>
            <Divider />
            <Box sx={{ py: 3, px: 5 }}>


            <ModalUserInfo/>

              <Box>
                <TextField
                  required
                  fullWidth
                  sx={{ marginY: '5px' }}
                  id='date'
                  label='Travel Date'
                  type='date'
                  defaultValue='YYYY-MM-DD'
                  InputLabelProps={{
                    shrink: true
                  }}
                />

                <TextField sx={{ marginY: '5px' }} fullWidth id='outlined-basic' label='Duration' variant='outlined' />
                <TextField sx={{ marginY: '5px' }} fullWidth id='outlined-basic' label='Location' variant='outlined' />
                <TextField
                  sx={{ marginY: '5px' }}
                  fullWidth
                  id='outlined-multiline-static'
                  label='Travel Reason'
                  multiline
                  rows={4}
                />
                <TextField
                  type='number'
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
                  <Box sx={{ width: '700px', display: 'flex', justifyContent: 'space-between' }}>
                    <Box>
                      <Typography>File Name : {} </Typography>
                    </Box>
                    <Box>
                      <Typography>File Size : {} </Typography>
                    </Box>
                  </Box>
                </Box>
              </Box>
              <Box sx={{ display: 'flex', mt: 15, mb: 5, justifyContent: 'flex-end', gap: 3 }}>
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
