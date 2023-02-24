import * as React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
import NoteTagAddModal from './NoteTagAddModal'
import TagsTable from './table/TagsTable'
import LocalOfferIcon from '@mui/icons-material/LocalOffer'


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
  mb: 3,
  borderRadius: '10px'
} 

export default function TagsModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <div>
      <Box onClick={handleOpen} sx={{ flex: 0.75, mx: '5px', display: 'flex', flexDirection: 'column', alignItems: 'center', cursor:"pointer" }}> <small>Tags</small>
                <LocalOfferIcon sx={{ color: 'lightslategray' }} fontSize='large' />
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
                {' '}
                Tags
              </Typography>

              <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
               
                <NoteTagAddModal/>


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
            <TagsTable/>
          </Box>
        </Box>
      </Modal>
    </div>
  )
}
