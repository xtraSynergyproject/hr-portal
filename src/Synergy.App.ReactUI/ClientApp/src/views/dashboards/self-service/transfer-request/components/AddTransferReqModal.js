import React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
// import TextField from '@mui/material/TextField'
// import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
// import Icon from 'src/@core/components/icon'
// import MenuBtn from './MenuBtn'
// import { Card, Divider } from '@mui/material'
// import Select from '@mui/material/Select'
// import MenuItem from '@mui/material/MenuItem'
// import InputLabel from '@mui/material/InputLabel'
// import FormControl from '@mui/material/FormControl'
// import MoreDetailsModal from './MoreDetailsModal'

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

function AddTransferReqModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <Box>
      <Button variant='contained' onClick={handleOpen}>
        New Transfer Request
      </Button>

      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby='modal-modal-title'
        aria-describedby='modal-modal-description'
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>jhghgfjtj</Box>
        </Box>
      </Modal>
    </Box>
  )
}

export default AddTransferReqModal
