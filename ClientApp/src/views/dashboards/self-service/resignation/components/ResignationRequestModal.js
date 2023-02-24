import React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import TextField from '@mui/material/TextField'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
import MenuBtn from 'src/views/dashboards/hr-only/termination/components/button_with_modal/MenuBtn'
import { Card, Divider } from '@mui/material'
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

function ResignationRequestModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <Box>
      <Button variant='contained' onClick={handleOpen}>
        Resignation Request
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
                Termination Request
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

            <Box sx={{ m: 4 }}>
                <ModalUserInfo />
          
            </Box>


          </Box>
        </Box>
      </Modal>
    </Box>
  )
}

export default ResignationRequestModal
