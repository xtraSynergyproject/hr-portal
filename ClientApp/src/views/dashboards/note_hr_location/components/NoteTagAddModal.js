import * as React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'

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
  width: '55rem',
  height: '35rem',
  mb: 3,
  borderRadius: '10px'
}

export default function NoteTagAddModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <div>
      <Typography
                  sx={{mx:"10px",
                    borderRadius: '50px',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    fontSize: '13px',
                    cursor: 'pointer'
                  }}
                  onClick={handleOpen}
                  component='label'
                >
                  <Icon icon='mdi:pencil-plus' fontSize='18px' />
                  Add Tag
                </Typography>




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
              Add Tags
              </Typography>

              <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
              <Typography
                  sx={{mx:"10px",
                    borderRadius: '50px',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    fontSize: '13px',
                    cursor: 'pointer'
                  }}
                  component='label'
                >
                  <Icon icon='mdi:plus' fontSize='18px' />
                  Add
                </Typography>


                <Typography
                  sx={{mx:"10px",
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
            </Box>
            <hr />

            
          </Box>
        </Box>
      </Modal>
    </div>
  )
}
