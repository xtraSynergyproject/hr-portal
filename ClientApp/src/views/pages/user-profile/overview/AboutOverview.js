// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import Card from '@mui/material/Card'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import Box from '@mui/material/Box'
import Grid from '@mui/material/Grid'
// // ** Icon Imports
import GroupsIcon from '@mui/icons-material/Groups'
import MenuIcon from '@mui/icons-material/Menu'
import Paper from 'src/@core/theme/overrides/paper'

const Overview = () => {
  return (
    // <Card>
    //   <CardContent>
        <Grid container spacing={12}>
          <Grid  item xs={12} sx={{ display: 'flex', justifyContent: 'flex-start' }}>
            <Card
              sx={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                m: 5,
                height: '200px',
                width: '300px'
              }}
            >
              <CardContent>
                <div>
                  <Box sx={{ display: 'flex', justifyContent: 'center', flexDirection: 'column' }}>
                    <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2 }}>
                      <GroupsIcon fontSize='medium' />
                      <Typography>
                        <b> 2</b>
                      </Typography>
                    </Box>
                    <Box>
                      <Typography fontSize='12px'>Teams</Typography>
                    </Box>
                  </Box>
                </div>
              </CardContent>
            </Card>

            <Card
              sx={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                m: 5,
                height: '200px',
                width: '300px'
              }}
            >
              <CardContent>
                <div>
                  <Box sx={{ display: 'flex', justifyContent: 'center', flexDirection: 'column' }}>
                    <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2 }}>
                      <MenuIcon fontSize='medium' />
                      <Typography>
                        <b> 130</b>
                      </Typography>
                    </Box>
                    <Box>
                      <Typography fontSize='12px'>Active Tasks</Typography>
                    </Box>
                  </Box>
                </div>
              </CardContent>
            </Card>
          </Grid>
        </Grid>
    //   </CardContent>
    // </Card>
  )
}

export default Overview

// // ** MUI Components
// import Box from '@mui/material/Box'
// import Grid from '@mui/material/Grid'
// import Card from '@mui/material/Card'
// import Typography from '@mui/material/Typography'
// import CardContent from '@mui/material/CardContent'

// // ** Icon Imports
// import Icon from 'src/@core/components/icon'
// import GroupsIcon from '@mui/icons-material/Groups'
// import MenuIcon from '@mui/icons-material/Menu'

// const renderList = arr => {
//   if (arr && arr.length) {
//     return arr.map((item, index) => {
//       return (
//         <Box
//           key={index}
//           sx={{
//             display: 'flex',
//             alignItems: 'center',
//             '&:not(:last-of-type)': { mb: 4 },
//             '& svg': { color: 'text.secondary' }
//           }}
//         >
//           <Icon icon={item.icon} />

//           <Typography sx={{ mx: 2, fontWeight: 600, color: 'text.secondary' }}>
//             {`${item.property.charAt(0).toUpperCase() + item.property.slice(1)}:`}
//           </Typography>
//           <Typography sx={{ color: 'text.secondary' }}>
//             {item.value.charAt(0).toUpperCase() + item.value.slice(1)}
//           </Typography>
//         </Box>
//       )
//     })
//   } else {
//     return null
//   }
// }

// const renderTeams = arr => {
//   if (arr && arr.length) {
//     return arr.map((item, index) => {
//       return (
//         <Box
//           key={index}
//           sx={{
//             display: 'flex',
//             alignItems: 'center',
//             // '&:not(:last-of-type)': { mb: 4 },
//             '& svg': { color: `${item.color}.main` }
//           }}
//         >
//           <Icon icon='item.icon' />

//           <Typography sx={{ mx: 2, fontWeight: 600, color: 'text.secondary' }}>
//             {item.property.charAt(0).toUpperCase() + item.property.slice(1)}
//           </Typography>
//           <Typography sx={{ color: 'text.secondary' }}>
//             {item.value.charAt(0).toUpperCase() + item.value.slice(1)}
//           </Typography>
//         </Box>
//       )
//     })
//   } else {
//     return null
//   }
// }

// const AboutOverview = props => {
//   const { teams, about, contacts, overview } = props

//   return (
//     <Grid container spacing={12}>
//       <Grid item xs={16} sx={{ display: 'flex', justifyContent: 'flex-start' }}>
//         <Card
//           sx={{
//             display: 'flex',
//             justifyContent: 'center',
//             alignItems: 'center',
//             m: 5,
//             height: '200px',
//             width: '600px'
//           }}
//         >
//           <CardContent>
//             <div>
//               <Box sx={{ display: 'flex', justifyContent: 'center', flexDirection: 'column' }}>
//                 <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2 }}>
//                   <GroupsIcon fontSize='medium' />
//                   <Typography>
//                     <b> 2</b>
//                   </Typography>
//                 </Box>
//                 <Box>
//                   <Typography fontSize='12px'>Teams</Typography>
//                 </Box>
//               </Box>
//             </div>
//           </CardContent>
//         </Card>

//         <Card
//           sx={{
//             display: 'flex',
//             justifyContent: 'center',
//             alignItems: 'center',
//             m: 5,
//             height: '200px',
//             width: '600px'
//           }}
//         >
//           <CardContent>
//             <div>
//               <Box sx={{ display: 'flex', justifyContent: 'center', flexDirection: 'column' }}>
//                 <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2 }}>
//                   <MenuIcon fontSize='medium' />
//                   <Typography>
//                     <b> 130</b>
//                   </Typography>
//                 </Box>
//                 <Box>
//                   <Typography fontSize='12px'>Active Tasks</Typography>
//                 </Box>
//               </Box>
//             </div>
//           </CardContent>
//         </Card>
//       </Grid>
//     </Grid>
//   )
// }

// export default AboutOverview
