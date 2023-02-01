// import React, {useState} from 'react'
// import Box from '@mui/material/Box'
// import Button from '@mui/material/Button'
// import Modal from '@mui/material/Modal'
// import { DesktopDateTimePicker } from '@mui/x-date-pickers/DesktopDateTimePicker';
// import dayjs from 'dayjs';
// import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';



// const modalWrapper = {
//   overflow: 'auto',
//   maxHeight: '100vh',
//   display: 'flex'
// }

// const modalBlock = {
//   position: 'relative',
//   zIndex: 0,
//   display: 'flex',
//   alignItems: 'center',
//   justifyContent: 'center',
//   margin: 'auto'
// }

// const modalContentStyle = {
//   position: 'relative',
//   background: '#fff',
//   boxShadow: 24,
//   mt: 3,
//   width: '20rem',
//   mb: 3,
//   borderRadius: '10px'
// }

// function MoreDetailsModal() {
//   const [open, setOpen] = useState(false)
//   const handleOpen = () => setOpen(true)
//   const handleClose = () => setOpen(false)
//   const [value, setValue] = useState(dayjs('2018-01-01T00:00:00.000Z'));

//   return (
//     <div>
//       <Button onClick={handleOpen}>Open modal</Button>

//       <Modal
//         open={open}
//         sx={modalWrapper}
//         onClose={handleClose}
//         aria-labelledby='modal-modal-title'
//         aria-describedby='modal-modal-description'
//       >
//         <Box sx={modalBlock}>
//           <Box sx={modalContentStyle}>
//             <Box>
//             <LocalizationProvider dateAdapter={AdapterDayjs}>

//             <DateTimePicker
//           label="Responsive"
//           renderInput={(params) => <TextField {...params} />}
//           value={value}
//           onChange={(newValue) => {
//             setValue(newValue);
//           }}
//         /></LocalizationProvider>
//             </Box>
//           </Box>
//         </Box>
//       </Modal>
//     </div>
//   )
// }

// export default MoreDetailsModal
