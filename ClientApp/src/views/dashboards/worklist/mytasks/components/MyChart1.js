// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import CardHeader from '@mui/material/CardHeader'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import CustomAvatar from 'src/@core/components/mui/avatar'
import TextSnippetIcon from '@mui/icons-material/TextSnippet';

// ** Third Party Imports
import { Tooltip, PieChart, Pie, Cell, ResponsiveContainer } from 'recharts'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

const data = [
  { name: 'reject', value:16, color: '#00d4bd' },
  { name: 'completed', value: 20, color: '#ffe700' },
  { name: 'pending', value: 80, color: '#FFA1A1' },
]
const RADIAN = Math.PI / 180

const renderCustomizedLabel = props => {
  // ** Props
  const { cx, cy, midAngle, innerRadius, outerRadius, percent } = props
  const radius = innerRadius + (outerRadius - innerRadius) * 0.5
  const x = cx + radius * Math.cos(-midAngle * RADIAN)
  const y = cy + radius * Math.sin(-midAngle * RADIAN)

  return (
    <text x={x} y={y} fill='#fff' textAnchor='middle' dominantBaseline='central'>
      {`${(percent * 100).toFixed(0)}%`}
    </text>
  )
}

const MyChart1 = () => {
  return (
      <>
    <Typography variant='h6' component='h2' sx={{ml:30,mt:-55}}>Leave</Typography>
    <Card  sx={{width:'26rem',
    height:'35vh',
    mt:7,
    ml:-60,
    border:'1px solid grey'
    }}>
      {/* <CardHeader
        title='Core HR'
        sx={{textAlign:'center',mt:-}}
        // subheader='Spending on various categories'
        // subheaderTypographyProps={{ sx: { color: theme => `${theme.palette.text.disabled} !important` } }}
      />  */}
      <CardContent>
         <Box sx={{
          // border:'1px solid grey',
          width:'7rem',
          height:'10rem'
        }}
        >
        
        <CustomAvatar variant='Circle' sx={{height:'100px',width:'100px'}}>
         <TextSnippetIcon sx={{weight:'20rem',height:'30vh'}}/>
              
        </CustomAvatar>

        {/* <Box> */}
         <Typography variant='h6' component='h2'textAlign={'center'}>4</Typography>
          Total Task(s)
         {/* </Box> */}
        </Box>
         
        <Box sx={{ height:'10rem',
        width:'10rem',
        mt:-45, 
        ml:50, 
        // border:'1px solid'
        }}>
           <ResponsiveContainer>
            <PieChart height={350} >
              <Pie data={data} innerRadius={0} dataKey='value' label={renderCustomizedLabel} labelLine={false}>
                {data.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={entry.color} />
                ))}
              </Pie>
              <Tooltip />
            </PieChart>
           </ResponsiveContainer> 
        </Box>
        <Box sx={{ display: 'flex', 
        flexWrap: 'wrap',
         mt: 1,
         ml:35,
        //  border:'1px solid '
         }}>
          <Box
            sx={{
              mr: 1,
              display: 'flex',
              alignItems: 'center',
              '& svg': { mr: 1.5, color: '#00d4bd' }
            }}
          >
            <Icon icon='mdi:circle' fontSize='0.75rem' />
            <Typography variant='body2'sx={{fontSize:12}}>Pending</Typography>
          </Box>
          <Box
            sx={{
              mr: 6,
              display: 'flex',
               alignItems: 'center',
              '& svg': { mr: 1.5, color: '#ffe700' }
            }}
          >
            <Icon icon='mdi:circle' fontSize='0.75rem' />
            <Typography variant='body2' sx={{fontSize:12}}>completed</Typography>
          </Box>
          <Box
            sx={{
              mr: 1,
              display: 'flex',
              alignItems: 'center',
              '& svg': { mr:1.5,ml:-4, color: '#FFA1A1' }
            }}
          >
            <Icon icon='mdi:circle' fontSize='0.75rem' />
            <Typography variant='body2' sx={{fontSize:12}}>Rejected</Typography>
          </Box>
          </Box> 

      </CardContent>
    </Card>
    </>
  )
}


export default MyChart1
