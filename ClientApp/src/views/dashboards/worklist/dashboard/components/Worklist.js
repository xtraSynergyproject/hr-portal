
import React from 'react'
import Typography from '@mui/material/Typography';
import { Box, Grid } from '@mui/material';
import WorklistTask from './WorklistTask';
import WorklistCreateService from './WorklistCreateService';
 import WorklistCreateNotes from './WorklistCreateNotes';
import WorklistCreateTasks from './WorklistCreateTasks';
 import WorklistNote from './WorklistNote';
import WorklistService from './WorklistService';
import WorklistforSubordinate from './WorklistforSubordinate';

export default function Worklist() {
  return (
    <div>
      <Box sx={{ justifyContent: 'space-between' }}>
        <Typography id="modal-modal-title" variant="h6" component="h4">
          WorkList
        </Typography>
        <Box>
          {/* <Paper evevation={0} */}
          <Box
          //  <Paper elevation={0} 
          
           
            sx={{
              width:'53rem',
              height: '4vh',
              backgroundColor: 'primary.dark',
              borderRadius: '5px',
              boxShadow:"5px 5px 10px rgba(255,255,255,0.8)"

            }}
          />
          {/* /> */}

        </Box>
        <WorklistTask />
        <WorklistCreateTasks />
         <WorklistService />
         <WorklistCreateService />
         <WorklistNote /> 
        <WorklistCreateNotes />
        <WorklistforSubordinate />
        <WorklistTask /> 
      </Box>
    </div>
  )
}

// // ** MUI Imports
// import Card from '@mui/material/Card'
// // import Button from '@mui/material/Button'
// // import Typography from '@mui/material/Typography'
// import CardHeader from '@mui/material/CardHeader'
// import CardContent from '@mui/material/CardContent'
// // import CardActions from '@mui/material/CardActions'

// const Worklist = () => {
//   return (
//     <Card>
//       <CardContent>
//         <Typography variant='body2' sx={{ mb: 3.25 }}>
//         <Box sx={{ justifyContent: 'space-between' }}>
//          <Typography id="modal-modal-title" variant="h6" component="h4">
//            WorkList
//          </Typography>
//          <Box>
//            {/* <Paper evevation={0} */}
//            <Box
//           //  <Paper elevation={0} 
//              className='freebox'
           
//              sx={{
//                width:'53rem',
//                height: '4vh',
//                backgroundColor: 'primary.dark',
//                borderRadius: '5px',
//                boxShadow:"5px 5px 10px rgba(255,255,255,0.8)"

//             }}
//            />

//          </Box>
//          <WorklistTask />
//          <WorklistCreateTasks />
//           <WorklistService />
//           <WorklistCreateService />
//           <WorklistNote /> 
//          <WorklistCreateNotes />
//         <WorklistforSubordinate />
//          <WorklistTask /> 
//     </Box>
//         </Typography>
       
//         {/* // <Typography variant='body2'>
//         //   If you’re in the market for new desktops, notebooks, or PDAs, there are a myriad of choices. Here’s a rundown
//         //   of some of the best systems available.
//           // </Typography> */}
//       </CardContent>
//       {/*// <CardActions className='card-action-dense'>
//       //   <Button>Read More</Button>
//       // </CardActions> */}
//     </Card>
//   )
// }

// export default Worklist
