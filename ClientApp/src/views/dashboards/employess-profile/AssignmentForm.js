
// export default PaySlip
import { useState, useEffect } from 'react'
// ** MUI Components
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import { styled } from '@mui/material/styles'
import CardMedia from '@mui/material/CardMedia'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import Assingmentmodal from './PersonalInfo/Assingmentmodal'
// ** api Imports
import axios from 'axios'


const UserProfileHeader = () => {
  // ** State
  const [data, setData] = useState(null)
  useEffect(() => {
    axios.get('/pages/profile-header').then(response => {
      setData(response.data)
    })
  }, [])


  // ** State
  const [datta, seetData] = useState(null)
  useEffect(() => {
    axios
      .get(
        `https://webapidev.aitalkx.com/chr/hrdirect/Assignment?userId=60da8f8f195197515042a1f2&personId=0a11a928-4f66-41b4-aa44-150d1470ef7e ` 
      )
      .then(response => {
        setData(response.data)
        localStorage.setItem('userProfile', JSON.stringify(response.data))
        console.log(response.data, 'profile data')
      })
  }, [])
  console.log(data, 'data')
  const designationIcon = data?.designationIcon || 'mdi:briefcase-outline'

  return data !== null ? (
    <>
     

<Card sx={{ display: "flex" }} className="user_profile_grid">
<Box sx={{ minWidth: '60%', m: 2, }}>
  <Typography sx={{ display: 'flex', mx: 2, mb: 2, fontSize: 18 }}>
    {/* <PersonOutlinedIcon /> */}
    <Box sx={{ ml: 1 }}>
      <h4>Assignment</h4>
    </Box>
  </Typography>
</Box>
<Box sx={{ m: 2, }}>
  <Typography sx={{ display: 'flex', m: 5, }}>

    <Box > 
    <Assingmentmodal />
    </Box>
  </Typography>
</Box>
</Card>

<Card sx={{py:"10px",display:"flex"}} className="user_profile_grid">
      <Box sx={{ minWidth: '6%', display:"flex", justifyContent:"center" }}>
       
      </Box>
      
      <Box className='user_profile_box box_one' sx={{  minWidth: '20%' ,mb:2}}>
        <Typography sx={{ display: 'flex', mx: 2, mb:2 }}>
          
          <Box sx={{ ml: 3 }}>
          Department:
          </Box>
         
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
      
          <Box sx={{ ml: 3 }}>
          Job:
          </Box>
      
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          Position:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
      
          <Box sx={{ ml: 3 }}>
            
          Location:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          Probation Period:
          </Box>
        </Typography>
      </Box>
      <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.PositionName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.JobName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.LocationName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.Mobile} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.PersonalEmail} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
          <b>{data.GradeName} </b>
          </Box>
        </Typography>
      </Box>
      <Box className='user_profile_box box_two' sx={{  minWidth: '15%' }}>
        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
         Assignment Grade:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
          Assignment Type:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
            Assignment Status:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
          Date Of Join:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
          Notice Period:
          </Box>
        </Typography>

      </Box>

      <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.GradeName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
           <b>{data.AssignmentTypeName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.GradeName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
            <b>{data.DateOfJoin} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
            
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
            
          </Box>
        </Typography>
      </Box>

    </Card>
 <Box sx={{ width: '100%', display: 'flex', ml: { xs: 0, md: 6 }, alignItems: 'flex-end', flexWrap: ['wrap', 'nowrap'], justifyContent: ['center', 'space-between'] }}>


</Box>
    </>
    
    
  ) : null
}

export default UserProfileHeader



