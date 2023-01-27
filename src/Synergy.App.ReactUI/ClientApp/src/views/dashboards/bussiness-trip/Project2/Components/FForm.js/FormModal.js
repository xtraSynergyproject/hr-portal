import React from 'react'
import { Modal } from '@mui/material'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import InputForm from '../../InputForm'
// import PersonalInfoForm from './PersonalInfoForm'
// import SubButton from './SubButton'
// import AddressInfoForm from './AddressInfoForm'

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
  width: '70%',
  mb: 3,
  borderRadius: '10px'
}

function FormModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <div>
      <Button onClick={handleOpen} sx={{color : "white"}}>Add Employee</Button>
      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby='modal-modal-title'
        aria-describedby='modal-modal-description'
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>
            {/* (<PersonalInfoForm />
            <AddressInfoForm />
            <SubButton />)  */}
            {/* Upper 3 tags are from old original form */}

            <InputForm/>
          </Box>
        </Box>
      </Modal>
    </div>
  )
}

export default FormModal
