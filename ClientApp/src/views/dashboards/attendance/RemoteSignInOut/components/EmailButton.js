// // import React from 'react'
// import React from 'react'

// function EmailButton() {
//   return (
//     <div>
//       Email
//     </div>
//   )
// }

// export default EmailButton
import * as React from 'react'
//import { Icon } from '@iconify/react';

import Box from '@mui/material/Box'

import Button from '@mui/material/Button'

import Typography from '@mui/material/Typography'
import Icon from 'src/@core/components/icon'

import Modal from '@mui/material/Modal'

import TextField from '@mui/material/TextField'
import { Divider, FormControl, InputLabel, Select, MenuItem, value } from '@mui/material'
// import MenuButton from './MenuButton'
// import Comment from './Comment'
import { comment } from 'stylis'
import { Save } from '@mui/icons-material'
// import MenuBtn from './MenuBtn'

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

export default function OtherReimbModal() {
  const [value, setValue] = React.useState('')
  const handleChanges = event => {
    setValue(event.target.value)
  }
  const [open, setOpen] = React.useState(false, Save)

  const handleOpen = () => setOpen(true)

  const handleClose = () => setOpen(false)
  const handleBack = () => setOpen(false)
//   const handleComment = () => setOpen(false)

  return (
    <div>
      <Typography onClick={handleOpen}>
        {/* <Icon icon='akar-icons:comment-add' /> */}Email
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
            <Box sx={{ mb: 2, display: 'flex', justifyContent: 'space-between' }} className='demo-space-x'>
              <Typography sx={{ p: 4 }} variant='h4' component='h3'>
                Email 
              </Typography>

              <Button
                sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }}
                onClick={handleClose}
                component='label'
              >
                <Icon icon='mdi:close' />
                Close
              </Button>
            </Box>

            <Box sx={{ py: 3, px: 5 }}>
              <Box>
                <TextField required fullWidth sx={{ borderRadius: '50px' }} />
              </Box>
              <Box sx={{ display: 'flex', mt: 15, mb: 5, justifyContent: 'flex-end', gap: 3 }}>
                <Button variant='contained'>
                  Mail
                </Button>
                <Button variant='contained' onClick={handleBack}>
                  Back
                </Button>
              </Box>
            </Box>
          </Box>
        </Box>
      </Modal>
    </div>
  )
}


// // import Icon from 'src/@core/components/icon'
// // import Button from '@mui/material/Button'

// // function Email() {
// //     const [open, setOpen] = React.useState(false)

// //   const handleOpen = () => setOpen(true)
// //   return (
// //     <div>
// //          <Button onClick={handleOpen}>
// //          <Icon icon='mdi:email' />
// //       </Button>
// //       <h3>Email</h3>
// //     </div>
// //   )
// // }

// // export default Email
// import Card from '@mui/material/Card'
// import Grid from '@mui/material/Grid'
// // import Divider from '@mui/material/Divider'
// import { DataGrid } from '@mui/x-data-grid'
// import * as React from 'react'
// //import { Icon } from '@iconify/react';

// import Box from '@mui/material/Box'

// import Button from '@mui/material/Button'

// import Typography from '@mui/material/Typography'
// import Icon from 'src/@core/components/icon'

// import Modal from '@mui/material/Modal'

// import TextField from '@mui/material/TextField'
// import { Divider, FormControl, InputLabel, Select, MenuItem, value } from '@mui/material'
// import MenuButton from './MenuButton'
// import Comment from './Comment'
// import { comment } from 'stylis'
// import { Save } from '@mui/icons-material'
// // import MenuBtn from './MenuBtn'

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

//   width: '50rem',

//   mb: 3,

//   borderRadius: '10px'
// }

// export default function OtherReimbModal() {
//   const [value, setValue] = React.useState('')
//   const handleChanges = event => {
//     setValue(event.target.value)
//   }
//   const [getdata, setGetdata] = useState([])
//   // const viewData = async () => { setGetdata}
//   const [open, setOpen] = React.useState(false, Save)

//   const handleOpen = () => setOpen(true)

//   const handleClose = () => setOpen(false)
//   const handleBack = () => setOpen(false)
//   const handleComment = () => setOpen(false)

//   return (
//     <div>
//       <Button onClick={handleOpen}>
//         <Icon icon='mdi:email' />
//       </Button>

//       <Modal
//         open={open}
//         sx={modalWrapper}
//         onClose={handleClose}
//         aria-labelledby='modal-modal-title'
//         aria-describedby='modal-modal-description'
//       >
//         <Box sx={modalBlock}>
//           <Box sx={modalContentStyle}>
//             <Box sx={{ mb: 2, display: 'flex', justifyContent: 'space-between' }} className='demo-space-x'>
//               <Typography sx={{ p: 4 }} variant='h4' component='h3'>
//                 Email
//               </Typography>

//               <Button
//                 sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }}
//                 onClick={handleClose}
//                 component='label'
//               >
//                 <Icon icon='mdi:close' />
//                 Close
//               </Button>
//             </Box>
//             <Box>
//               <Button variant='contained' /* onClick={handleOpen */>Create New Email</Button>
//               <Button variant='contained' onClick={handleBack}>
//                 Refresh
//               </Button>
//             </Box>
//             <Grid item xs={12}>
//        <Card>
//           <DataGrid
//          autoHeight
//          rows={getdata}
//                 columns={columns}
//               // checkboxSelection
//               pageSize={pageSize}
//               disableSelectionOnClick
//         rowsPerPageOptions={[10,15,50]}
//           onPageSizeChange={newPageSize => setPageSize(newPageSize)}
//             getRowId={row => row.Id}
//           />
//          </Card>
//         </Grid>
//             <Box sx={{ py: 3, px: 5 }}>
//               {/* <Box>
//                 <TextField required fullWidth sx={{ borderRadius: '50px' }} />
//               </Box> */}
//               <Box sx={{ display: 'flex', mt: 15, mb: 5, justifyContent: 'flex-end', gap: 3 }}></Box>
//             </Box>
//           </Box>
//         </Box>
//       </Modal>
//     </div>
//   )
// }
